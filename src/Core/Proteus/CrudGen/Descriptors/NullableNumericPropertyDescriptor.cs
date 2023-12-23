using TheXDS.Proteus.CrudGen.Descriptions;

namespace TheXDS.Proteus.CrudGen.Descriptors;

/// <summary>
/// Implements a <see cref="PropertyDescriptor"/> for numeric properties.
/// </summary>
/// <typeparam name="T">Type of property to describe.</typeparam>
public class NullableNumericPropertyDescriptor<T> : PropertyDescriptor, INumericPropertyDescriptor<T>, INullablePropertyDescription, INumericPropertyDescription<T> where T : unmanaged, IComparable<T>
{
}