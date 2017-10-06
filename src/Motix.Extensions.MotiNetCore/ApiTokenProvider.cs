using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MotiNet
{
    public class ApiTokenProvider : IApiTokenProvider
    {
        public void GenerateToken(string secretKey, string command, string data, double timeOut, out long timeStamp, out string token)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            var password = $"{secretKey}{command}{data}";

            timeStamp = DateTime.Now.AddMilliseconds(timeOut).Ticks;
            token = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(timeStamp.ToString()),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public bool ValidateToken(string secretKey, string command, string data, long timeStamp, string token)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            DateTime time;
            try
            {
                time = new DateTime(timeStamp);
            }
            catch
            {
                throw new ArgumentException(nameof(timeStamp));
            }

            if (time < DateTime.Now)
            {
                return false;
            }

            var password = $"{secretKey}{command}{data}";

            return token == Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(timeStamp.ToString()),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
