﻿/*
Copyright © 2017-2019 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Crud.Base;
using Xceed.Wpf.Toolkit;

namespace TheXDS.Proteus.Crud.Mappings
{
    public class DecimalUpDownMapping : XceedNumericMapping<DecimalUpDown, decimal>
    {
        public DecimalUpDownMapping(IPropertyDescription property) : base(property)
        {            
        }
    }
}