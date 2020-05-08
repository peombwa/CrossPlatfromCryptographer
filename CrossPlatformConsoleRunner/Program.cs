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

                byte[] readData = null;
                byte[] token = null;
                string originalContent = null;
                for (int i = 0; i < 1000; i++)
                {
                    token = Encoding.UTF8.GetBytes($"{sampleData} - {i + 1}");
                    LinuxCryptographer.AddOrUpdate(appId, token);
                    readData = LinuxCryptographer.Get(appId);
                    originalContent = Encoding.UTF8.GetString(readData);
                    Console.WriteLine($"Decypted content: {originalContent}");
                }
                                
                LinuxCryptographer.Remove(appId);
                readData = LinuxCryptographer.Get(appId);
                originalContent = Encoding.UTF8.GetString(readData);
                Console.WriteLine($"Decypted content: {originalContent}");   
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Device doesn't support secure storage. {ex.Message}");
            }
        }
    }
}
