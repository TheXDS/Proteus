using System;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class GiftCard : TimestampModel<long>
    {
        public decimal Amount { get; set; }
        public DateTime? Void { get; set; }
    }
}