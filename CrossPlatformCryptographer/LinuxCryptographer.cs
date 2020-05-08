namespace CrossPlatformCryptographer
{
    using CrossPlatformCryptographer.NativeLibs;
    using System;
    using System.Runtime.InteropServices;
    using static CrossPlatformCryptographer.NativeLibs.LinuxNativeLibKeyUtil;

    public static class LinuxCryptographer
    {
        private const string ServiceName = "graph_powershell_sdk";
        public static void AddOrUpdate(string appId, byte[] plainContent)
        {
            if (plainContent != null && plainContent.Length > 0){
                string encodedContent = Convert.ToBase64String(plainContent);
                int key = LinuxNativeLibKeyUtil.request_key(KeyTypes.User, $"{ServiceName}:{appId}", IntPtr.Zero, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
                if (key == -1)
                    key = LinuxNativeLibKeyUtil.add_key(KeyTypes.User, $"{ServiceName}:{appId}", encodedContent, encodedContent.Length, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
                else
                    LinuxNativeLibKeyUtil.keyctl_update(key, encodedContent, encodedContent.Length);
            }
        }

        public static byte[] Get(string appId)
        {
            int key = LinuxNativeLibKeyUtil.request_key(KeyTypes.User, $"{ServiceName}:{appId}", IntPtr.Zero, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
            if (key == -1)
                return new byte[0];

            int contentLength = LinuxNativeLibKeyUtil.keyctl_read_alloc(key, out IntPtr contentPtr);
            string content = Marshal.PtrToStringAuto(contentPtr);
            Marshal.FreeHGlobal(contentPtr);

            if (String.IsNullOrEmpty(content))
                return new byte[0];

            return Convert.FromBase64String(content);
        }

        public static void Remove(string appId)
        {
            int key = LinuxNativeLibKeyUtil.request_key(KeyTypes.User, $"{ServiceName}:{appId}", IntPtr.Zero, (int)KeyringType.KEY_SPEC_USER_SESSION_KEYRING);
            if (key == -1)
                return ;
                
            int removedState = LinuxNativeLibKeyUtil.keyctl_revoke(key);
            if (removedState == -1)
                throw new Exception("Failed to remove token from cache.");
        }
    }
}