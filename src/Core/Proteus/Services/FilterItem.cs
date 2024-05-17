using TheXDS.MCART.Types.Base;
using TheXDS.Proteus.CrudGen.Descriptions;
using TheXDS.Proteus.Services.Base;

namespace TheXDS.Proteus.Services;

/// <summary>
/// Includes information on query filters that can be applied to an
/// <see cref="IEntityProvider"/> when fetching data.
/// </summary>
/// <param name="property">Property for which to filter.</param>
/// <param name="query">Query string.</param>
public class FilterItem(IPropertyDescription? property, string? query) : NotifyPropertyChanged
{
    private IPropertyDescription? property = property;
    private string? query = query;

    /// <summary>
    /// Property for which to filter.
    /// </summary>
    public IPropertyDescription? Property
    {
        get => property;
        set
        {
            if (value is not null) Change(ref property, value);
        }
    }

    /// <summary>
    /// Query string.
    /// </summary>
    public string? Query
    {
        get => query;
        set => Change(ref query, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterItem"/> class.
    /// </summary>
    public FilterItem() : this(null, null)
    {
    }
}
