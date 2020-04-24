using CrossPlatformCryptographer;
using System;

namespace CrossPlatformConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = "NotSecureToken";
            Console.WriteLine($"Plain content: {token}");
            try
            {
                Console.WriteLine("Encypting...");
                Cryptographer.Encrypt(token).GetAwaiter().GetResult();
                Console.WriteLine("Encrypted");
                Console.WriteLine("Decypting...");
                string decryptedToken = Cryptographer.Decrypt(token).GetAwaiter().GetResult();
                Console.WriteLine($"Decypted content: {decryptedToken}");
                Console.WriteLine("Removing key...");
                Cryptographer.Remove();
                Console.WriteLine("Removed");
                Console.WriteLine("Decypting...");
                decryptedToken = Cryptographer.Decrypt(token).GetAwaiter().GetResult();
                Console.WriteLine($"Decypted content: {decryptedToken}");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Device doesn't support secure storage. {ex.Message}");
            }
        }
    }
}
