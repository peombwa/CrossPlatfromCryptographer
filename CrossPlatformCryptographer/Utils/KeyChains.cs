using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CrossPlatformCryptographer.Utils
{
    internal class KeyChains
    {
        internal class Errors
        {
            public const int Success = 0;
            public const long ItemNotFound = -25300;
            public const long KeychainDenied = -128;
            public const long SecAuthFailed = -25293;
            public const long PlistMissing = -67030;
        }
        private const string SecurityFramework = "/System/Library/Frameworks/Security.framework/Security";
        private const string FoundationFramework = "/System/Library/Frameworks/Foundation.framework/Foundation";

        public enum CFStringEncoding : uint
        {
            UTF16 = 0x0100,
            UTF16BE = 0x10000100,
            UTF16LE = 0x14000100,
            ASCII = 0x0600
        }

        public unsafe static IntPtr CreateCFString(string aString)
        {
            var bytes = Encoding.Unicode.GetBytes(aString);
            fixed (byte* b = bytes)
            {
                var cfStr = CFStringCreateWithBytes(IntPtr.Zero, (IntPtr)b, bytes.Length, CFStringEncoding.UTF16, false);
                return cfStr;
            }
        }

        // warning: this doesn't call retain/release on the elements in the array
        public unsafe static IntPtr CreateCFArray(IntPtr[] objectes)
        {
            fixed (IntPtr* vals = objectes)
            {
                return CFArrayCreate(IntPtr.Zero, (IntPtr)vals, objectes.Length, IntPtr.Zero);
            }
        }

        [DllImport(FoundationFramework)]
        public static extern IntPtr CFStringCreateWithBytes(IntPtr allocator, IntPtr buffer, long bufferLength, CFStringEncoding encoding, bool isExternalRepresentation);

        [DllImport(FoundationFramework)]
        public static extern IntPtr CFArrayCreate(IntPtr allocator, IntPtr values, long numValues, IntPtr callbackStruct);

        [DllImport(FoundationFramework)]
        public static extern void CFRetain(IntPtr handle);

        [DllImport(FoundationFramework)]
        public static extern void CFRelease(IntPtr handle);

        [DllImport(SecurityFramework)]
        public static extern string SecKeychainOpen(string service, string username);

        [DllImport(SecurityFramework)]
        public static extern IntPtr SecKeychainFindGenericPassword(IntPtr keychainOrArray, int serviceNameLength, string serviceName, int accountNameLength, string accountName, IntPtr passwordLength, IntPtr passwordData, out IntPtr itemRef);

        [DllImport(SecurityFramework)]
        public static extern IntPtr SecKeychainFindGenericPassword(IntPtr keychainOrArray, int serviceNameLength, string serviceName, int accountNameLength, string accountName, out int passwordLength, out IntPtr passwordData, IntPtr itemRef);

        [DllImport(SecurityFramework)]
        public static extern IntPtr SecKeychainAddGenericPassword(IntPtr keychain, int serviceNameLength, string serviceName, int accountNameLength, string accountName, int passwordLength, string passwordData, IntPtr itemRef);

        [DllImport(SecurityFramework)]
        public static extern IntPtr SecKeychainItemModifyAttributesAndData(IntPtr itemRef, IntPtr attrList, int passwordLength, string passwordData);

        [DllImport(SecurityFramework)]
        public static extern IntPtr SecKeychainItemDelete(IntPtr itemRef);
    }
}
