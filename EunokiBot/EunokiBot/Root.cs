using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EunokiBot
{
    public class Root
    {
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;

            if (Suspender.IsSuspended)
                return false;

            SQL.Singleton.UpdateValue(GetTableName(), propertyName, value, GetUserID());

            return true;
        }

        protected virtual string GetTableName()
        {
            return null;
        }
        protected virtual ulong GetUserID()
        {
            return 0;
        }
    }
}
