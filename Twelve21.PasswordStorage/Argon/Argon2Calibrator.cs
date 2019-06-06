using System;
using System.Collections.Generic;
using System.Diagnostics;
using Twelve21.PasswordStorage.Utilities;

namespace Twelve21.PasswordStorage.Argon
{
    public class Argon2Calibrator
    {
        private Argon2Factory _argon2Factory;
        private Argon2Logger _logger;
        private Argon2CalibrationInput _input;

        public Argon2Calibrator(
            Argon2Factory argon2Factory,
            Argon2Logger logger,
            Argon2CalibrationInput input)
        {
            _argon2Factory = argon2Factory ?? throw new ArgumentNullException(nameof(argon2Factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _input = input ?? throw new ArgumentNullException(nameof(input));
        }

        public IEnumerable<Argon2CalibrationResult> Run()
        {
            //
            // password and salt will be 128-bits, which is sufficient
            // for all applications [Section 9].
            //
            var password = new string('0', _input.SaltAndPasswordLength);
            var salt = new string('1', _input.SaltAndPasswordLength);

            //
            // the maximum time it should take to calculate the
            // password hash.
            //
            var maximumTime = _input.MaximumTime;

            //
            // degree of parallelism should be twice the number of
            // cpu cores [Section 6.4]
            //
            var degreeOfParallelism = _input.DegreeOfParallelism;

            //
            // we will start at 1MB and work our way up to an acceptable
            // level. this memory usage is specified in KB.
            //
            var results = new List<Argon2CalibrationResult>();

            for (int memoryUsage = 1024; memoryUsage <= 4 * 1024 * 1024; memoryUsage *= 2)
            {
                //
                // figure out the maximum number of iterations such that the
                // running time does not exceed the maximum time. if the
                // running time exceeds the maximum time for a single iteration
                // then reduce the memory usage accordingly and try again.
                //
                Argon2CalibrationResult bestResult = null;
                var iterationSearch = new ExponentialSearch(_input.MinimumIterations, int.MaxValue, iterations =>
                {
                    var parameters = new Argon2Parameters()
                    {
                        DegreeOfParallelism = degreeOfParallelism,
                        Iterations = iterations,
                        MemoryUsage = memoryUsage
                    };

                    //
                    // argon2id is preferred for password storage.
                    //
                    var argon2 = _argon2Factory.Create(_input.Mode, password, salt, parameters);
                    _logger.WriteBeginCalibrationTest(parameters);

                    var stopwatch = Stopwatch.StartNew();
                    argon2.GetBytes(_input.HashLength);
                    stopwatch.Stop();

                    var elapsedTime = stopwatch.ElapsedMilliseconds;
                    _logger.WriteCompleteCalibrationTest(elapsedTime);

                    //
                    // store off the best (slowest) number of iterations for this memory usage.
                    //
                    if (elapsedTime <= maximumTime && (bestResult == null || elapsedTime > bestResult.ElapsedMilliseconds))
                    {
                        bestResult = new Argon2CalibrationResult()
                        {
                            ElapsedMilliseconds = elapsedTime,
                            Parameters = parameters
                        };
                    }

                    if (elapsedTime > maximumTime)
                        return ExponentialSearchComparison.ToHigh;
                    else if (elapsedTime < maximumTime)
                        return ExponentialSearchComparison.ToLow;
                    else
                        return ExponentialSearchComparison.Equal;
                });

                //
                // if there was a best result for this memory usage and
                // iteration combo, then store for later consumption.
                //
                iterationSearch.Search();
                if (bestResult != null)
                    results.Add(bestResult);
            }

            //
            // highest usage of memory and thread count should be recommended
            // first. we'll reverse the results so the consumer receives
            // them in recommended order.
            //
            results.Reverse();
            return results;
        }
    }
}
