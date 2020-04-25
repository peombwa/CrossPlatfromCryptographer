using CrossPlatformCryptographer;
using System;
using System.Diagnostics;

namespace CrossPlatformConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {

            string appId = Guid.NewGuid().ToString();
            string token = "NotSecureToken";
            Console.WriteLine($"Plain content: {token}");
            try
            {
                Console.WriteLine("Encypting...");
                MacOSSecurityCommand.Add(appId, token);
                Console.WriteLine("Encrypted");
                Console.WriteLine("Decypting...");
                string decryptedToken = MacOSSecurityCommand.Get(appId);
                Console.WriteLine($"Decypted content: {decryptedToken}");
                Console.WriteLine("Removing key...");
                MacOSSecurityCommand.Remove(appId);
                Console.WriteLine("Removed");
                Console.WriteLine("Decypting...");
                decryptedToken = MacOSSecurityCommand.Get(appId);
                Console.WriteLine($"Decypted content: {decryptedToken}");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Device doesn't support secure storage. {ex.Message}");
            }
        }
    }
}
