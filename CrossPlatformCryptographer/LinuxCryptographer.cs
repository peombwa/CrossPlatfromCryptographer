namespace CrossPlatformCryptographer
{
    using System;
    using System.Runtime.InteropServices;
    using CrossPlatformCryptographer.Utils;
    public static class LinuxCryptographer
    {
        private const string ServiceName = "graph_powershell_sdk";
        public static void AddorUpdate(string appId, byte[] plainContent)
        {
            if (plainContent != null && plainContent.Length > 0){
                string encodedContent = Convert.ToBase64String(plainContent);
                int key = LibKeyUtils.request_key(KeyTypes.User, $"{ServiceName}:{appId}", ServiceName, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
                if (key == -1)
                    key = LibKeyUtils.add_key(KeyTypes.User, $"{ServiceName}:{appId}", encodedContent, encodedContent.Length, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
                else
                    LibKeyUtils.keyctl_update(key, encodedContent, encodedContent.Length);
            }
        }

        public static byte[] Get(string appId)
        {
            int key = LibKeyUtils.request_key(KeyTypes.User, $"{ServiceName}:{appId}", ServiceName, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
            if (key == -1)
                return new byte[0];

            long contentLength = LibKeyUtils.keyctl_read_alloc(key, out IntPtr contentPtr);
            string content = Marshal.PtrToStringAuto(contentPtr);
            Marshal.FreeHGlobal(contentPtr);

            if (String.IsNullOrEmpty(content))
                return new byte[0];

            return Convert.FromBase64String(content);
        }

        public static void Remove(string appId)
        {
            int key = LibKeyUtils.request_key(KeyTypes.User, $"{ServiceName}:{appId}", ServiceName, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
            if (key == -1)
                throw new Exception("Access token not found in cache.");
                
            long removedState = LibKeyUtils.keyctl_revoke(key);
            if (removedState == -1)
                throw new Exception("Failed to remove token from cache.");
        }
    }
}