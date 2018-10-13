using PocBlazor.Model;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PocBlazor.Authentication
{
    public class BearerTokenGenerator
    {
        public static string GetBearerToken(ConsumerCredentials userCredentials, string webMethod, string url)
        {
            C4MAuthentication auth = new C4MAuthentication
            {
                ConsumerKey = userCredentials.ConsumerKey,
                Nonce = GetNonce(),
                Timestamp = ToEpochTime(DateTime.Now)
            };

            auth.Signature = GetSignatureHash(userCredentials.ConsumerSecret, (userCredentials.ConsumerSecret +
                auth.ConsumerKey +
                auth.Nonce +
                auth.Timestamp +
                auth.Version +
                webMethod.ToUpperInvariant() +
                url.ToUpperInvariant()));

            //Newtonsoft json is not working properly yet with Blazor.. so doing manually..
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(auth.ToString()));
        }

        private static string GetSignatureHash(string key, string input)
        {
            using (var hmac256 = new HMACSHA256())
            {
                hmac256.Key = Encoding.UTF8.GetBytes(key);
                hmac256.Initialize();
                return
                    Convert.ToBase64String(
                        hmac256.ComputeHash(Encoding.UTF8.GetBytes(input)));
            }
        }

        private static string GetNonce()
        {
            var numRamdom = BitConverter.GetBytes(GetRandomNumber(1, 1000));
            return Convert.ToBase64String(numRamdom);
        }

        private static int GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return Convert.ToInt32(random.NextDouble() * (maximum - minimum) + minimum);
        }

        private static long ToEpochTime(DateTime dateTime)
        {
            DateTime date = dateTime.ToUniversalTime();
            TimeSpan ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
