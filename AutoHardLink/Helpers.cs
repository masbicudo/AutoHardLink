using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace AutoHardLink
{
    internal class Helpers
    {
        public static string SHA256CheckSum(string filePath)
        {
            using (SHA256 SHA256 = SHA256.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                    return Convert.ToBase64String(SHA256.ComputeHash(fileStream));
            }
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateHardLink(
                string lpFileName,
                string lpExistingFileName,
                IntPtr lpSecurityAttributes
            );
    }
}
