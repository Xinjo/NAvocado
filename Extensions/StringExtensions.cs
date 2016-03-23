using System.Security.Cryptography;
using System.Text;

namespace NAvocado.Extensions
{
    public static class StringExtensions
    {
        public static string ToSHA256(this string value)
        {
            var sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var encoding = Encoding.UTF8;
                var bytes = hash.ComputeHash(encoding.GetBytes(value));

                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}