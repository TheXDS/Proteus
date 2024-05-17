using TheXDS.Proteus.CrudGen;

namespace TheXDS.Proteus.Types;

/// <summary>
/// TODO: Implements the required logic to define a navigatable Proteus Module.
/// </summary>
public abstract class ProteusModule
{
    private readonly ModuleItem[] _items = [];

    /// <summary>
    /// Gets a reference to all the menu items exposed by a module.
    /// </summary>
    public IEnumerable<IGrouping<CrudCategory, ICrudDescription>> MenuItems
        => _items
        .SelectMany(p => p.Descriptions)
        .Where(p => p.Category.HasValue)
        .GroupBy(p => p.Category!.Value);
}
