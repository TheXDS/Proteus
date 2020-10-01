/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Models
{
    public class Setting : ModelBase<string>
    {
        public string Value { get; set; }
        public virtual ConfigRepository Parent { get; set; }

        [NotMapped]
        public Type DataType { get; internal set; } = typeof(string);
        [NotMapped]
        public int IntValue
        {
            get => int.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public byte ByteValue
        {
            get => byte.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public short ShortValue
        {
            get => short.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public long LongValue
        {
            get => long.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public float FloatValue
        {
            get => float.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public double DoubleValue
        {
            get => double.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public decimal DecimalValue
        {
            get => decimal.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public DateTime DateTimeValue
        {
            get => DateTime.TryParse(Value, out var v) ? v : default;
            set => Value = value.ToString();
        }
        [NotMapped]
        public bool BoolValue
        {
            get => bool.TryParse(Value, out var v) && v;
            set => Value = value.ToString();
        }
        [NotMapped]
        public Enum EnumValue
        {
            get => Enum.TryParse(DataType, Value, out var v) ? (Enum)v! : (Enum)DataType.Default()!;
            set => Value = value?.ToString() ?? DataType.Default()!.ToString()!;
        }

        public IEnumerable<NamedObject<Enum>> EnumValues => DataType.IsEnum ? Enum.GetValues(DataType).Cast<Enum>().Select(p => new NamedObject<Enum>(p, p.NameOf())) : Array.Empty<NamedObject<Enum>>();

        public static implicit operator KeyValuePair<string, string>(Setting setting) => new KeyValuePair<string, string>(setting.Id, setting.Value);
        public static implicit operator Setting(KeyValuePair<string, string> value) => new Setting { Id = value.Key, Value = value.Value };
    }
}