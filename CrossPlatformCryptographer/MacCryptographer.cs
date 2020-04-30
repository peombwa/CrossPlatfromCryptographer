using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CrossPlatformCryptographer.Utils;

namespace CrossPlatformCryptographer
{
    public static class MacCryptographer
    {
        private const string ServiceName = "com.microsoft.graph.powershell.sdkcache";
        public static void AddOrUpdate(string appId, byte[] plainContent)
        {
            UTF8Encoding utf8Encoding = new UTF8Encoding(true, true);
            string encodedContent = utf8Encoding.GetString(plainContent);
            IntPtr resultStatus = KeyChains.SecKeychainFindGenericPassword(
                IntPtr.Zero, ServiceName.Length, ServiceName, appId.Length, appId, IntPtr.Zero, IntPtr.Zero, out IntPtr itemRef);

            if (resultStatus.ToInt64() == KeyChains.Errors.Success)
            {
                resultStatus = KeyChains.SecKeychainItemModifyAttributesAndData(itemRef, IntPtr.Zero, encodedContent.Length, encodedContent);
                Marshal.FreeHGlobal(itemRef);
            }
            else
            {
                resultStatus = KeyChains.SecKeychainAddGenericPassword(IntPtr.Zero, ServiceName.Length, ServiceName, appId.Length, appId, encodedContent.Length, encodedContent, IntPtr.Zero);
            }
        }

        public static byte[] Get(string appId)
        {
            IntPtr resultStatus = KeyChains.SecKeychainFindGenericPassword(
                IntPtr.Zero, ServiceName.Length, ServiceName, appId.Length, appId, out int contentPtrLen, out IntPtr contentPtr, IntPtr.Zero);

            byte[] decodedContent = null;
            if(resultStatus.ToInt64() == 0 && contentPtrLen > 0) {
                string content = Marshal.PtrToStringAuto(contentPtr);
                Marshal.FreeHGlobal(contentPtr);

                if (string.IsNullOrEmpty(content))
                    return new byte[0];
                UTF8Encoding utf8Encoding = new UTF8Encoding(true, true);
                decodedContent = utf8Encoding.GetBytes(content);
            }
            long resLong = resultStatus.ToInt64();
            return decodedContent;
        }

        public static void Remove(string appId)
        {
            IntPtr resultStatus = KeyChains.SecKeychainFindGenericPassword(
                IntPtr.Zero, ServiceName.Length, ServiceName, appId.Length, appId, IntPtr.Zero, IntPtr.Zero, out IntPtr itemRef);

            if (resultStatus.ToInt64() == 0)
            {
                resultStatus = KeyChains.SecKeychainItemDelete(itemRef);                
                Marshal.FreeHGlobal(itemRef);
            }
        }
    }
}
 