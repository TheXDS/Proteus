﻿using System.Windows.Input;
using TheXDS.Ganymede.Services;
using TheXDS.Ganymede.Types.Base;
using TheXDS.Proteus.CrudGen;
using TheXDS.Triton.Models.Base;

namespace TheXDS.Proteus.Services.Base;

/// <summary>
/// Defines a set of members to be implemented by a type that provides query
/// and search services for collections of entities.
/// </summary>
public interface IEntityProvider : IViewModel
{
    /// <summary>
    /// Gets the current query results.
    /// </summary>
    /// <remarks>
    /// This collection will be populated whenever the
    /// <see cref="FetchDataAsync"/> method is invoked.
    /// </remarks>
    IEnumerable<Model> Results { get; }

    /// <summary>
    /// Gets the total number of items that exists in the query.
    /// </summary>
    int TotalItems { get; }

    /// <summary>
    /// Gets or sets the desired page number to put into the
    /// <see cref="Results"/> collection.
    /// </summary>
    int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page to put into the
    /// <see cref="Results"/> collection.
    /// </summary>
    int ItemsPerPage { get; set; }

    /// <summary>
    /// Gets the total number of pages that can be returned into the
    /// <see cref="Results"/> collection.
    /// </summary>
    int TotalPages => ItemsPerPage > 0 ? (int)Math.Ceiling(TotalItems / (float)ItemsPerPage) : 1;

    /// <summary>
    /// Gets a collection of search filters that can be applied to the query.
    /// </summary>
    IDictionary<ICrudDescription, Filter> Filters { get; }

    /// <summary>
    /// Gets a reference to the command used to navigate to the first page.
    /// </summary>
    ICommand FirstPageCommand { get; }

    /// <summary>
    /// Gets a reference to the command used to navigate to the last page.
    /// </summary>
    ICommand LastPageCommand { get; }

    /// <summary>
    /// Gets a reference to the command used to navigate to the next page.
    /// </summary>
    ICommand NextPageCommand { get; }

    /// <summary>
    /// Gets a reference to the command used to navigate to the previous page.
    /// </summary>
    ICommand PreviousPageCommand { get; }

    /// <summary>
    /// Gets a reference to the command used to reload the contents of the
    /// <see cref="Results"/> collection.
    /// </summary>
    ICommand RefreshCommand { get; }

    /// <summary>
    /// Gets a reference to the command used to edit the current filter selection.
    /// </summary>
    ICommand EditFiltersCommand { get; }

    /// <summary>
    /// Gets a reference to the command used to clear the filter list.
    /// </summary>
    ICommand ClearFiltersCommand { get; }

    /// <summary>
    /// Gets a reference to the models being returned by this provider.
    /// </summary>
    ICrudDescription[] Models { get; }

    INavigationService? IViewModel.NavigationService { get => null; set { } }

    string? IViewModel.Title { get => null; set { } }

    /// <summary>
    /// Gets the count of filters currently being applied for all managed models.
    /// </summary>
    public int FiltersCount => Filters.Sum(p => p.Value.Items.Count);

    /// <summary>
    /// Creates the internal query used by this instance to generate the       
    /// results and executes it.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that can be used to await the async operation.
    /// </returns>
    Task FetchDataAsync();
}
