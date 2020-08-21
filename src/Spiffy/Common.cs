using System;

namespace Nhlpa.Sql
{
    public class Common
    {
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
