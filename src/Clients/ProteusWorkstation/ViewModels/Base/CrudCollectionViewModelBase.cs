﻿/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Extensions;
using TheXDS.MCART.ViewModel;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models.Base;
using P = System.Windows.Controls.Primitives;

namespace TheXDS.Proteus.ViewModels.Base
{

    /// <summary>
    /// ViewModel que administra las operaciones de Crud con elementos de UI autogenerados.
    /// </summary>
    public abstract class CrudCollectionViewModelBase : CrudViewModelBase, ICrudCollectionViewModel
    {
        private ObservableCollectionWrap<ModelBase> _source = null!;

        /// <summary>
        /// Contiene una lista personalizada de columnas a mostrar.
        /// </summary>
        protected List<IColumn> CustomColumns { get; } = new List<IColumn>();

        /// <summary>
        /// Obtiene al elemento selector de la ventana.
        /// </summary>
        public ItemsControl Selector { get; }

        /// <summary>
        /// Obtiene un <see cref="ViewBase"/> que define la apariencia de
        /// un selector <see cref="ListView"/> cuando esta ventana de CRUD
        /// controla únicamente un modelo de datos.
        /// </summary>
        public virtual ViewBase? ColumnsView
        {
            get
            {
                var v = new GridView();
                foreach (var j in (CrudElement.GetDescription(Models.First())?.ListColumns ?? CustomColumns).OfType<Column>())
                {
                    v.Columns.Add(j);
                }
                return v;
            }
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="CrudCollectionViewModelBase"/>.
        /// </summary>
        /// <param name="parentModel">
        /// Modelo padre de la propiedad para la cual se está generando el
        /// Crud.
        /// </param>
        /// <param name="source">Colección de orígen a controlar.</param>
        /// <param name="elements">Elementos de edición a incorporar.</param>
        /// <param name="csource">
        /// Propiedad de origen a utilizar para el selector de entidades.
        /// </param>
        protected CrudCollectionViewModelBase(Type? parentModel, ICollection<ModelBase> source, Type[] elements, string csource = "Results") : base(elements)
        {
            ParentModel = parentModel;
            if (elements.Count() == 1)
            {
                Selector = new ListView();
                Selector.SetBinding(P.Selector.SelectedItemProperty, new Binding(nameof(Selection))) ;
                Selector.SetBinding(ListView.ViewProperty, new Binding(nameof(ColumnsView)));
            }
            else
            {
                // HACK: TreeView es una perra
                var bg = ((Brush)Application.Current.TryFindResource("SystemAltHighColorBrush")).Clone();
                bg.Opacity = 0.5;

                Selector = new TreeView()
                {
                    Background = bg,
                    ItemTemplateSelector = new ModelTemplateSelector(Elements.Select(p => p.Description))
                };
                ((TreeView)Selector).SelectedItemChanged += TreeViewSelector_SelectionChanged;
            }
            Source = new ObservableCollectionWrap<ModelBase>(source);
            Selector.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(csource)
            {
                Mode = BindingMode.OneWay
            });
            RegisterPropertyChangeBroadcast(nameof(Selection), nameof(ColumnsView));
        }

        private void TreeViewSelector_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Selection = ((TreeView)Selector).SelectedItem as ModelBase;
        }

        /// <summary>
        /// Enumera el orígen de datos establecido para este Crud.
        /// </summary>
        public ObservableCollectionWrap<ModelBase> Source
        {
            get => _source;
            set => Change(ref _source, value);
        }

        ICollection<ModelBase> ICrudCollectionViewModel.Source => Source;

        /// <summary>
        /// Obtiene a la entidad padre de la entidad actualmente
        /// seleccionada.
        /// </summary>
        /// <returns>
        /// El padre de la entidad seleccionada, o <see langword="null"/>
        /// si no es posible determinar a un padre en este contexto.
        /// </returns>
        protected override ModelBase? GetParent()
        {
            return Selector switch
            {
                TreeView trv => trv.SelectedItem as ModelBase,
                Control v => WalkUp(v) switch
                {
                    ICrudViewModel vm=> vm.Selection,
                    IEntityViewModel vm => vm.Entity,
                    _ => null,
                } as ModelBase
            };
        }

        public object? WalkUp(Control v)
        {
            var vm = v.DataContext;
            if (vm is IEntityViewModel) return vm;
            FrameworkElement? c = v;
            while (c?.DataContext == vm) c = c.Parent as FrameworkElement;
            return c?.DataContext;
        }

        /// <summary>
        /// Ejecuta acciones posteriores al guardado de una entidad en la base de datos.
        /// </summary>
        protected override async Task PostSave(ModelBase e)
        {
            await base.PostSave(e);
            Notify(nameof(Source));
        }
    }
}