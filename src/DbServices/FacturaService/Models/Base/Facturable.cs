﻿using System.Collections.Generic;

namespace TheXDS.Proteus.Models.Base
{
    public abstract class Facturable : Nameable<string>
    {
        public decimal Precio { get; set; }
        public virtual List<ItemFactura> Instances { get; set; } = new List<ItemFactura>();
        public virtual FacturableCategory? Category { get; set; }
        public float? Isv { get; set; }
        public virtual List<AutomationItem> Automations { get; set; } = new List<AutomationItem>();
    }
}
