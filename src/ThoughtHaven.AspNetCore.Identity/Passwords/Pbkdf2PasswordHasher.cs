using System;
using System.Security.Cryptography;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private static readonly int HashSize = 256 / 8;
        private static readonly int SaltSize = 128 / 8;
        protected int Iterations { get; }

        public Pbkdf2PasswordHasher(int iterations = 25_000)
        {
            Guard.LessThan(nameof(iterations), iterations, minimum: 1);

            this.Iterations = iterations;
        }

        public string Hash(Password password)
        {
            Guard.Null(nameof(password), password);

            using (var cipher = new Rfc2898DeriveBytes(
                password.Value, SaltSize, this.Iterations))
            {
                var hash = cipher.GetBytes(HashSize);

                var concat = Concat(cipher.Salt, hash);

                return $"{this.Iterations}.{Convert.ToBase64String(concat)}";
            }
        }

        public PasswordValidateResult Validate(string hash, Password password)
        {
            Guard.NullOrWhiteSpace(nameof(hash), hash);
            Guard.Null(nameof(password), password);

            try
            {
                var split = hash.Split(new string[] { "." }, StringSplitOptions.None);

                var iterations = int.Parse(split[0]);
                var base64Concat = split[1];
                var concat = Convert.FromBase64String(base64Concat);

                byte[] salt = new byte[SaltSize];
                for (var i = 0; i < SaltSize; i++) { salt[i] = concat[i]; }

                using (var cipher = new Rfc2898DeriveBytes(password.Value, salt, iterations))
                {
                    var testHash = cipher.GetBytes(HashSize);

                    concat = Concat(salt, testHash);

                    var valid = base64Concat == Convert.ToBase64String(concat);

                    return new PasswordValidateResult(valid,
                        updateHash: valid && this.Iterations > iterations);
                }
            }
            catch
            {
                return new PasswordValidateResult(valid: false);
            }
        }

        private static byte[] Concat(byte[] salt, byte[] hash)
        {
            Guard.Null(nameof(salt), salt);
            Guard.Null(nameof(hash), hash);

            var concat = new byte[salt.Length + hash.Length];

            for (var i = 0; i < salt.Length; i++)
            {
                concat[i] = salt[i];
            }

            for (var i = 0; i < hash.Length; i++)
            {
                concat[i + salt.Length] = hash[i];
            }

            return concat;
        }
    }
}