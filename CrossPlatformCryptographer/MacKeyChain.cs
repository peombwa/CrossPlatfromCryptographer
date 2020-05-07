using CrossPlatformCryptographer.NativeLibs;
using System;
using System.Runtime.InteropServices;

namespace CrossPlatformCryptographer
{
    public static class MacKeyChain
    {
        private const string ServiceName = "com.microsoft.graph.powershell.sdkcache";
        public static void AddOrUpdate(string appId, byte[] plainContent)
        {
            IntPtr passwordDataPtr = IntPtr.Zero;
            IntPtr itemPtr = IntPtr.Zero;
            try
            {
                int resultStatus = MacNativeKeyChain.SecKeychainFindGenericPassword(
                    keychainOrArray: IntPtr.Zero,
                    serviceNameLength: (uint)ServiceName.Length,
                    serviceName: ServiceName,
                    accountNameLength: (uint)appId.Length,
                    accountName: appId,
                    passwordLength: out uint passwordLength,
                    passwordData: out passwordDataPtr,
                    itemRef: out itemPtr);

                if (resultStatus != MacNativeKeyChain.SecResultCodes.errSecSuccess &&
                    resultStatus != MacNativeKeyChain.SecResultCodes.errSecItemNotFound)
                    throw new Exception($"SecKeychainFindGenericPassword failed with {resultStatus}");

                if(itemPtr != IntPtr.Zero)
                {
                    // Key exists, let's update it.
                    resultStatus = MacNativeKeyChain.SecKeychainItemModifyAttributesAndData(
                        itemRef: itemPtr,
                        attrList: IntPtr.Zero,
                        passwordLength: (uint)plainContent.Length,
                        passwordData: plainContent);

                    if (resultStatus != MacNativeKeyChain.SecResultCodes.errSecSuccess)
                        throw new Exception($"SecKeychainItemModifyAttributesAndData failed with {resultStatus}");
                }
                else
                {
                    // Key not found, let's create a new one in the default keychain.
                    resultStatus = MacNativeKeyChain.SecKeychainAddGenericPassword(
                        keychain: IntPtr.Zero,
                        serviceNameLength: (uint)ServiceName.Length,
                        serviceName: ServiceName,
                        accountNameLength: (uint)appId.Length,
                        accountName: appId,
                        passwordLength: (uint)plainContent.Length,
                        passwordData: plainContent,
                        itemRef: out itemPtr);

                    if (resultStatus != MacNativeKeyChain.SecResultCodes.errSecSuccess)
                        throw new Exception($"SecKeychainAddGenericPassword failed with {resultStatus}");
                }
            }
            finally
            {
                FreePointers(ref itemPtr, ref passwordDataPtr);
            }
        }

        public static byte[] Get(string appId)
        {
            IntPtr passwordDataPtr = IntPtr.Zero;
            IntPtr itemPtr = IntPtr.Zero;

            try
            {
                byte[] contentBuffer = new byte[0];
                int resultStatus = MacNativeKeyChain.SecKeychainFindGenericPassword(
                    keychainOrArray: IntPtr.Zero,
                    serviceNameLength: (uint)ServiceName.Length,
                    serviceName: ServiceName,
                    accountNameLength: (uint)appId.Length,
                    accountName: appId,
                    passwordLength: out uint passwordLength,
                    passwordData: out passwordDataPtr,
                    itemRef: out itemPtr);

                if (resultStatus == MacNativeKeyChain.SecResultCodes.errSecItemNotFound)
                    return contentBuffer;

                else if (resultStatus != MacNativeKeyChain.SecResultCodes.errSecSuccess)
                    throw new Exception($"SecKeychainFindGenericPassword failed with {resultStatus}");

                if (itemPtr != IntPtr.Zero && passwordLength > 0)
                {
                    contentBuffer = new byte[passwordLength];
                    Marshal.Copy(passwordDataPtr, contentBuffer, 0, contentBuffer.Length);
                }
                return contentBuffer;
            }
            finally
            {
                FreePointers(ref itemPtr, ref passwordDataPtr);
            }
        }

        public static void Remove(string appId)
        {
            IntPtr passwordDataPtr = IntPtr.Zero;
            IntPtr itemPtr = IntPtr.Zero;

            try
            {
                int resultStatus = MacNativeKeyChain.SecKeychainFindGenericPassword(
                    keychainOrArray: IntPtr.Zero,
                    serviceNameLength: (uint)ServiceName.Length,
                    serviceName: ServiceName,
                    accountNameLength: (uint)appId.Length,
                    accountName: appId,
                    passwordLength: out uint passwordLength,
                    passwordData: out passwordDataPtr,
                    itemRef: out itemPtr);

                if (resultStatus == MacNativeKeyChain.SecResultCodes.errSecItemNotFound)
                    return ;
                else if (resultStatus != MacNativeKeyChain.SecResultCodes.errSecSuccess)
                    throw new Exception($"SecKeychainFindGenericPassword failed with {resultStatus}");

                if (itemPtr == IntPtr.Zero)
                    return;

                resultStatus = MacNativeKeyChain.SecKeychainItemDelete(itemPtr);
                if (resultStatus != MacNativeKeyChain.SecResultCodes.errSecSuccess)
                    throw new Exception($"SecKeychainItemDelete failed with {resultStatus}");
            }
            finally
            {
                FreePointers(ref itemPtr, ref passwordDataPtr);
            }
        }

        private static void FreePointers(ref IntPtr itemPtr, ref IntPtr passwordDataPtr)
        {
            if (itemPtr != IntPtr.Zero)
            {
                MacNativeKeyChain.CFRelease(itemPtr);
                itemPtr = IntPtr.Zero;
            }

            if (passwordDataPtr != IntPtr.Zero)
            {
                MacNativeKeyChain.SecKeychainItemFreeContent(attrList: IntPtr.Zero, data: passwordDataPtr);
                passwordDataPtr = IntPtr.Zero;
            }
        }
    }
}
 