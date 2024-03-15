using System.Text;
using System.Security.Cryptography;

namespace TURNERO.Helpers
{
    public static class HelperText
    {
        public static string Sha256(string input)
        {
            using (SHA256 sha256 = new SHA256Managed())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
   
}
