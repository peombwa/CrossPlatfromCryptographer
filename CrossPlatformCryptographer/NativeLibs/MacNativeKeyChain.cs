using System;
using System.Runtime.InteropServices;

namespace CrossPlatformCryptographer.NativeLibs
{
    /// <summary>
    /// Native calls to MacOS KeyChains Items API - https://developer.apple.com/documentation/security/keychain_services/keychain_items.
    /// </summary>
    internal class MacNativeKeyChain
    {
        /// <summary>
        /// https://developer.apple.com/documentation/security/1542001-security_framework_result_codes
        /// </summary>
        public static class SecResultCodes
        {
            public const int errSecSuccess = 0;
            public const int errSecAuthFailed = -25293;
            public const int errSecNoSuchKeychain = -25294;
            public const int errSecDuplicateKeychain = -25296;
            public const int errSecDuplicateItem = -25299;
            public const int errSecItemNotFound = -25300;
            public const int errSecNoDefaultKeychain = -25307;
            public const int errSecDecode = -26275;
        }

        private const string SecurityFramework = "/System/Library/Frameworks/Security.framework/Security";
        private const string FoundationFramework = "/System/Library/Frameworks/Foundation.framework/Foundation";

        [DllImport(FoundationFramework)]
        public static extern IntPtr CFArrayCreate(IntPtr allocator, IntPtr values, long numValues, IntPtr callbackStruct);

        [DllImport(FoundationFramework)]
        public static extern void CFRetain(IntPtr handle);

        [DllImport(FoundationFramework)]
        public static extern void CFRelease(IntPtr handle);

        [DllImport(SecurityFramework)]
        public static extern int SecKeychainItemFreeContent(IntPtr attrList, IntPtr data);

        [DllImport(SecurityFramework)]
        public static extern int SecKeychainFindGenericPassword(IntPtr keychainOrArray, uint serviceNameLength, string serviceName, uint accountNameLength, string accountName, out uint passwordLength, out IntPtr passwordData, out IntPtr itemRef);

        [DllImport(SecurityFramework)]
        public static extern int SecKeychainAddGenericPassword(IntPtr keychain, uint serviceNameLength, string serviceName, uint accountNameLength, string accountName, uint passwordLength, byte[] passwordData, out IntPtr itemRef);

        [DllImport(SecurityFramework)]
        public static extern int SecKeychainItemModifyAttributesAndData(IntPtr itemRef, IntPtr attrList, uint passwordLength, byte[] passwordData);

        [DllImport(SecurityFramework)]
        public static extern int SecKeychainItemDelete(IntPtr itemRef);
    }
}
