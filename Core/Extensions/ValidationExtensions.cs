using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Core.Extensions
{
    public static class ValidationExtensions
    {
        public static List<T> AssertNotNullOrEmpty<T>(this List<T> list, string parameterName)
        {
            if (list == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (list.Count == 0)
            {
                throw new ArgumentNullException(parameterName);
            }
            
            return list;
        }

        public static string AssertIsValidEmail(this string email, string parameterName)
        {
            try
            {
                var result = new MailAddress(email);
                return result.Address;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, parameterName);
            }
        }
    }
}