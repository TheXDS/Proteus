/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Component
{
    public class IdComparer<T> : IEqualityComparer<T> where T : ModelBase
    {
        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            return x?.StringId == y?.StringId;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.StringId?.GetHashCode() ?? 0;
        }
    }
}