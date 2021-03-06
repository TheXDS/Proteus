﻿/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;

namespace TheXDS.Proteus.Crud.Base
{
    public interface IListBasePropertyDescription
    {
        IEnumerable<Column> Columns { get; }
    }
}