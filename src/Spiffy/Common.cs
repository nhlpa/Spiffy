using System;

namespace Spiffy
{
    public class Common
    {
        /// <summary>
        /// Convert object value into type T, with support for nullable value types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ChangeType<T>(object value)
        {
            if (value == null || value == DBNull.Value)
                return default;

            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                t = Nullable.GetUnderlyingType(t);

            return (T)Convert.ChangeType(value, t);
        }
    }
}
