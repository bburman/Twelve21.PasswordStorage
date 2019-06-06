using Konscious.Security.Cryptography;
using System;
using System.Text;

namespace Twelve21.PasswordStorage.Argon
{
    public class Argon2Factory
    {
        public Argon2 Create(Argon2Mode mode, string password, string salt, Argon2Parameters parameters)
        {
            var argon2 = CreateInternal(mode, password);

            argon2.Salt = Encoding.UTF8.GetBytes(salt);
            argon2.DegreeOfParallelism = parameters.DegreeOfParallelism;
            argon2.Iterations = parameters.Iterations;
            argon2.MemorySize = parameters.MemoryUsage;

            return argon2;
        }

        private static Argon2 CreateInternal(Argon2Mode mode, string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            switch (mode)
            {
                case Argon2Mode.Argon2i:
                    return new Argon2i(passwordBytes);
                case Argon2Mode.Argon2d:
                    return new Argon2d(passwordBytes);
                case Argon2Mode.Argon2id:
                    return new Argon2id(passwordBytes);
                default:
                    throw new NotSupportedException($"The specified Argon2 mode, '{ mode }', is not supported.");
            }
        }
    }
}
