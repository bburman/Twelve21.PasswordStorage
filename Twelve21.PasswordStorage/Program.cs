using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Linq;
using Twelve21.PasswordStorage.Argon;
using Twelve21.PasswordStorage.Utilities;

namespace Twelve21.PasswordStorage
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Description = "Password storage utility.";
            app.HelpOption("-?|-h|--help");

            app.Command("a2c", command =>
            {
                command.Description = "Perform an Argon2 calibration to determine the best combination of iterations, memory usage, and parallel threads for password storage using Argon2.";
                command.HelpOption("-?|-h|--help");

                var timeOption = command.Option(
                    "-t|--time",
                    "The maximum time in milliseconds it should take to calculate the password hash. Defaults to 1000.",
                    CommandOptionType.SingleValue
                    );
                var parallelismOption = command.Option(
                    "-p|--parallelism",
                    "The degree of parallelism. Defaults to twice the number of CPU cores.",
                    CommandOptionType.SingleValue
                    );
                var iterationsOption = command.Option(
                    "-i|--iterations",
                    "The minimum number of iterations. Defaults to 10. Source see crypto analysis on https://en.wikipedia.org/wiki/Argon2",
                    CommandOptionType.SingleValue
                    );
                var modeOption = command.Option(
                    "-m|--mode",
                    "The mode of operation. The default is Argon2id. Advanced usage only.",
                    CommandOptionType.SingleValue,
                    c => c.ShowInHelpText = false
                    );
                var saltLengthOption = command.Option(
                    "--saltlength",
                    "The length of the salt and password, in bytes. Defaults to 16. Advanced usage only.",
                    CommandOptionType.SingleValue,
                    c => c.ShowInHelpText = false
                    );
                var hashLengthOption = command.Option(
                    "--hashlength",
                    "The length of the hash, in bytes. Defaults to 16. Advanced usage only.",
                    CommandOptionType.SingleValue,
                    c => c.ShowInHelpText = false
                    );

                command.OnExecute(() =>
                {
                    var factory = new Argon2Factory();
                    var logger = new Argon2Logger();
                    var input = new Argon2CalibrationInput()
                    {
                        MaximumTime = ReadOption(timeOption, () => 1000),
                        DegreeOfParallelism = ReadOption(parallelismOption, () => SystemManagement.GetTotalCpuCores() * 2),
                        MinimumIterations = ReadOption(iterationsOption, () => 10),
                        Mode = ReadOption(modeOption, () => Argon2Mode.Argon2id),
                        SaltAndPasswordLength = ReadOption(saltLengthOption, () => 16),
                        HashLength = ReadOption(hashLengthOption, () => 16)
                    };

                    var calibrator = new Argon2Calibrator(factory, logger, input);
                    var results = calibrator.Run();

                    logger.WriteLine();
                    logger.WriteLine("Best results:");
                    results.ToList().ForEach(result => logger.WriteCalibrationResult(result));

                    return 0;
                });
            });

            app.OnExecute(() => 0);
            app.Execute(args);
        }

        private static long ReadOption(CommandOption option, Func<long> defaultValue)
        {
            if (!option.HasValue())
                return defaultValue();

            return long.Parse(option.Value());
        }

        private static int ReadOption(CommandOption option, Func<int> defaultValue)
        {
            if (!option.HasValue())
                return defaultValue();

            return int.Parse(option.Value());
        }

        private static TEnum ReadOption<TEnum>(CommandOption option, Func<TEnum> defaultValue)
            where TEnum : struct
        {
            if (!option.HasValue())
                return defaultValue();

            return Enum.Parse<TEnum>(option.Value());
        }
    }
}
