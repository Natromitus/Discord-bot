using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EunokiBot.Model
{
    public abstract class Root
    {
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;

            if (ReadSuspender.IsSuspended)
                return false;

            if (WriteSuspender.IsSuspended)
                return true;

            if (value.GetType() == typeof(string))
            {
                string sCurVal = "'" + value + "'";
                SQL.Singleton.UpdateValue(OnGetTableName(), propertyName, sCurVal, OnGetPrimaryKeyName(), OnGetPrimaryKeyValue());
            }
            else
                SQL.Singleton.UpdateValue(OnGetTableName(), propertyName, value, OnGetPrimaryKeyName(), OnGetPrimaryKeyValue());

            return true;
        }

        protected abstract string OnGetTableName();
        protected abstract string OnGetPrimaryKeyName();
        protected abstract object OnGetPrimaryKeyValue();
    }
}
