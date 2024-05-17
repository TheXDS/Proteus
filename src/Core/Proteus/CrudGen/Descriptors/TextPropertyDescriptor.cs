using TheXDS.Proteus.CrudGen.Descriptions;

namespace TheXDS.Proteus.CrudGen.Descriptors;

/// <summary>
/// Implements a <see cref="PropertyDescriptor"/> for <see cref="string"/>
/// properties.
/// </summary>
public class TextPropertyDescriptor
    : PropertyDescriptor,
    ITextPropertyDescriptor,
    ITextPropertyDescription
{
}
