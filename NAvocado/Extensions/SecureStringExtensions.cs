using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NAvocado.Extensions
{
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Convert a <see cref="SecureString"/> to <see cref="string"/>
        /// </summary>
        /// <param name="secureString"><see cref="SecureString"/> to convert</param>
        /// <returns>A <see cref="string"/> representation of the <see cref="SecureString"/></returns>
        /// <remarks>Due to providing easy of use over security, and WPF databinding this method is used </remarks>
        public static string ConvertToUnsecureString(this SecureString secureString)
        {
            if (secureString == null)
            {
                throw new ArgumentNullException(nameof(secureString));
            }

            var unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}