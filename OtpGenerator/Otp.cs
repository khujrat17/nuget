using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OtpGenerator
{
    public class Otp
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        private static readonly char[] AlphanumericChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        // Keep track of recent OTPs to prevent immediate reuse
        private static readonly HashSet<string> RecentOtps = new();

        #region Basic OTP Generation (v3.1)

        // Generate numeric OTP
        public string GenerateOtp(int length = 6)
        {
            ValidateLength(length);
            var otpBuilder = new StringBuilder(length);
            byte[] randomNumber = new byte[1];

            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(randomNumber);
                otpBuilder.Append(randomNumber[0] % 10);
            }

            return otpBuilder.ToString();
        }

        // Generate unique OTP with timestamp
        public string GenerateUniqueOtp(int length = 6)
        {
            string otp = GenerateOtp(length);
            return $"{otp}-{DateTime.UtcNow.Ticks}";
        }

        // Generate multiple OTPs
        public string[] GenerateMultipleOtps(int count, int length = 6)
        {
            string[] otps = new string[count];
            for (int i = 0; i < count; i++)
            {
                otps[i] = GenerateOtp(length);
            }
            return otps;
        }

        // OTP with prefix
        public string GenerateOtpWithPrefix(string prefix, int length = 6)
        {
            string otp = GenerateOtp(length);
            return $"{prefix}-{otp}";
        }

        // OTP with suffix
        public string GenerateOtpWithSuffix(string suffix, int length = 6)
        {
            string otp = GenerateOtp(length);
            return $"{otp}-{suffix}";
        }

        // Hash OTP for secure storage
        public string HashOtp(string otp)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // Validate OTP
        public bool ValidateOtp(string generatedOtp, string userInput)
        {
            return generatedOtp.Equals(userInput);
        }

        #endregion

        #region Advanced v4.0 Features

        // Generate OTP with expiration
        public ExpiringOtp GenerateOtpWithExpiry(int length = 6, int expireMinutes = 5)
        {
            string code = GenerateOtp(length);
            return new ExpiringOtp
            {
                Code = code,
                Expiry = DateTime.UtcNow.AddMinutes(expireMinutes)
            };
        }

        // Generate alphanumeric OTP
        public string GenerateAlphanumericOtp(int length = 6)
        {
            ValidateLength(length);
            var otpBuilder = new StringBuilder(length);
            byte[] randomNumber = new byte[1];

            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(randomNumber);
                otpBuilder.Append(AlphanumericChars[randomNumber[0] % AlphanumericChars.Length]);
            }

            return otpBuilder.ToString();
        }

        // Generate unique OTP with no repeats
        public string GenerateUniqueOtpNoRepeat(int length = 6)
        {
            string otp;
            do
            {
                otp = GenerateOtp(length);
            } while (RecentOtps.Contains(otp));

            RecentOtps.Add(otp);
            return otp;
        }

        // OTP validator with retry limit
        public OtpValidator GetValidator(string otp, int maxAttempts = 3)
        {
            return new OtpValidator(otp, maxAttempts);
        }

        #endregion

        #region Helpers

        private void ValidateLength(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0");

            if (length > 10) // max 10 digits
                throw new ArgumentException("Length must not exceed 10");
        }

        #endregion
    }

    #region Supporting Classes

    // OTP with expiration
    public class ExpiringOtp
    {
        public string Code { get; set; }
        public DateTime Expiry { get; set; }

        public bool IsExpired() => DateTime.UtcNow > Expiry;
    }

    // Validator with retry limit
    public class OtpValidator
    {
        private readonly string _otp;
        private int _attempts;
        private readonly int _maxAttempts;

        public OtpValidator(string otp, int maxAttempts = 3)
        {
            _otp = otp;
            _maxAttempts = maxAttempts;
            _attempts = 0;
        }

        public bool Validate(string input)
        {
            if (_attempts >= _maxAttempts) return false;
            _attempts++;
            return _otp == input;
        }

        public int AttemptsLeft => _maxAttempts - _attempts;
    }

    #endregion
}
