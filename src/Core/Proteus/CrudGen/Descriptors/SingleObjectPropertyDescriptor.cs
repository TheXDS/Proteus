using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.CrudGen.Descriptors;

/// <summary>
/// Describes a property that holds a single <c><see cref="Model"/></c>
/// instance.
/// </summary>
public class SingleObjectPropertyDescriptor
    : PropertyDescriptor,
    ISingleObjectPropertyDescriptor,
    ISingleObjectPropertyDescription
{
}
