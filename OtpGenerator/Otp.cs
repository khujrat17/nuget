using System;
using System.Security.Cryptography;
using System.Text;
namespace OtpGenerator
{
    

    public class Otp
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        // Method to generate a random OTP of specified length
        public string GenerateOtp(int length = 6)
        {
            ValidateLength(length);
            var otpBuilder = new StringBuilder(length);
            byte[] randomNumber = new byte[1];

            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(randomNumber);
                otpBuilder.Append(randomNumber[0] % 10); // Generates a digit from 0 to 9
            }

            return otpBuilder.ToString();
        }

        // Method to validate the length of the OTP
        private void ValidateLength(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be greater than 0");
            }

            if (length > 10) // Assuming a maximum length of 10 for OTP
            {
                throw new ArgumentException("Length must not exceed 10");
            }
        }

        // Method to generate a unique OTP (for example, by appending a timestamp)
        public string GenerateUniqueOtp(int length = 6)
        {
            string otp = GenerateOtp(length);
            return $"{otp}-{DateTime.UtcNow.Ticks}"; // Example of making it unique
        }

        // Method to validate if the provided OTP matches the generated OTP
        public bool ValidateOtp(string generatedOtp, string userInput)
        {
            return generatedOtp.Equals(userInput);
        }

        // Method to generate a list of OTPs for testing or other purposes
        public string[] GenerateMultipleOtps(int count, int length = 6)
        {
            string[] otps = new string[count];
            for (int i = 0; i < count; i++)
            {
                otps[i] = GenerateOtp(length);
            }
            return otps;
        }

        // Method to generate an OTP with a specific prefix
        public string GenerateOtpWithPrefix(string prefix, int length = 6)
        {
            string otp = GenerateOtp(length);
            return $"{prefix}-{otp}";
        }

        // Method to generate an OTP with a specific suffix
        public string GenerateOtpWithSuffix(string suffix, int length = 6)
        {
            string otp = GenerateOtp(length);
            return $"{otp}-{suffix}";
        }



        // Method to generate a hash of the OTP (for secure storage)
        public string HashOtp(string otp)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
