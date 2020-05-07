using CrossPlatformCryptographer;
using System;
using System.Text;

namespace CrossPlatformConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {

            string appId = "4c3cfd28-c898-4900-94e8-a23f78baa0c7";
            string sampleData = "The quick brown fox jumped over the lazy dog.";
            byte[] token = Encoding.UTF8.GetBytes(sampleData);
            Console.WriteLine($"Plain content: {token.Length}");
            try
            {
                //byte[] previousContent = LinuxCryptographer.Get(appId);
                //Console.WriteLine($"Previous content length: {previousContent.Length}");
                //Console.WriteLine("Encypting...");
                //LinuxCryptographer.AddorUpdate(appId, token);
                //Console.WriteLine("Encrypted");

                //Console.WriteLine("Decypting...");
                //byte[] readData = LinuxCryptographer.Get(appId);
                //string originalContent = Encoding.UTF8.GetString(readData);
                //Console.WriteLine($"Decypted content: {originalContent}");
                //readData = null;
                //originalContent = string.Empty;

                //Console.WriteLine("Removing key...");
                //LinuxCryptographer.Remove(appId);
                //Console.WriteLine("Removed");

                //Console.WriteLine("Decypting...");
                //readData = LinuxCryptographer.Get(appId);
                //originalContent = Encoding.UTF8.GetString(readData);
                //Console.WriteLine($"Decypted content: {originalContent}");
                MacKeyChain.AddOrUpdate(appId, token);
                byte[] readData = MacKeyChain.Get(appId);
                string originalContent = Encoding.UTF8.GetString(readData);
                Console.WriteLine($"Decypted content: {originalContent}");
                MacKeyChain.Remove(appId);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Device doesn't support secure storage. {ex.Message}");

            }
        }
    }
}
