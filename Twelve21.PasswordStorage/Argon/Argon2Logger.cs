using System;

namespace Twelve21.PasswordStorage.Argon
{
    public class Argon2Logger
    {
        public void WriteBeginCalibrationTest(Argon2Parameters parameters)
        {
            Write($"M = {parameters.MemoryUsage / 1024,4} MB, ");
            Write($"T = {parameters.Iterations,4}, ");
            Write($"d = {parameters.DegreeOfParallelism}, ");
        }
        
        public void WriteCompleteCalibrationTest(long elapsedMilliseconds)
        {
            WriteLine($"Time = {elapsedMilliseconds / 1000.0} s");
        }

        public void WriteCalibrationResult(Argon2CalibrationResult result)
        {
            WriteBeginCalibrationTest(result.Parameters);
            WriteCompleteCalibrationTest(result.ElapsedMilliseconds);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}
