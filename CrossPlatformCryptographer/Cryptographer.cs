using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CrossPlatformCryptographer
{
    public static class Cryptographer
    {
        public static async Task Encrypt(string plainText)
        {
            await SecureStorage.SetAsync("msal_oauth_token", plainText).ConfigureAwait(false);
        }

        public static async Task<string> Decrypt(string cypherText)
        {
            return await SecureStorage.GetAsync("msal_oauth_token").ConfigureAwait(false);
        }

        public static void Remove()
        {
            SecureStorage.Remove("msal_oauth_token");
        }
    }
}
