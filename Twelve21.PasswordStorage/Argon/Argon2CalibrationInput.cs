namespace Twelve21.PasswordStorage.Argon
{
    public class Argon2CalibrationInput
    {
        public long MaximumTime { get; set; }

        public int DegreeOfParallelism { get; set; }

        public int MinimumIterations { get; set; }

        public Argon2Mode Mode { get; set; }

        public int SaltAndPasswordLength { get; set; }

        public int HashLength { get; set; }
    }
}
