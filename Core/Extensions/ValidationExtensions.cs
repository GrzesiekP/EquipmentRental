using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public static Guid AssertIsNotEmpty(this Guid guid, string parameterName)
        {
            return guid != Guid.Empty ?
                guid :
                throw new ArgumentNullException(parameterName);
        }
    }
}