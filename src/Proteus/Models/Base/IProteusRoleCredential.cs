﻿/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;

namespace TheXDS.Proteus.Models.Base
{
    public interface IProteusRoleCredential : IProteusCredential
    {
        IEnumerable<IProteusCredential> Members { get; }
    }
}