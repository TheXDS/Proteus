/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Crud.Base;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Reflection;
using TheXDS.Proteus.Models.Base;
using System.Windows.Data;
using System.Linq;
using TheXDS.MCART.Types;
using TheXDS.Proteus.ViewModels.Base;
using System.Runtime.CompilerServices;

namespace TheXDS.Proteus.Crud
{
    internal class InputSplashDescription : IPropertyDescription
    {
        public PropertyLocation PropertySource => PropertyLocation.ViewModel;
        public bool Hidden => false;
        public bool ReadOnly => false;
        public bool ShowInDetails => false;
        public bool ShowWatermark => true;
        public string ReadOnlyFormat => null;
        public NullMode Nullability => NullMode.Required;
        public Type PropertyType => Property.PropertyType;
        public string RadioGroup => null;
        public int? Order => null;
        public Func<ModelBase, PropertyInfo, IEnumerable<ValidationError>> Validator => null;
        public IDictionary<DependencyProperty, BindingBase> CustomBindings => null;
        public bool IsListColumn => false;
        public bool UseDefault => !(Default is null);
        public object Default { get; set; }
        public string Icon { get; set; }
        public string Label { get; set; }
        public PropertyInfo Property { get; set; }
        public string Tooltip { get; set; }
    }
    internal class ListInputSplashDescription : InputSplashDescription, IListPropertyDescription
    {
        public bool Editable => true;

        public bool Selectable => true;

        public BindingBase DisplayMemberBinding => null!;

        public string DisplayMemberPath => null!;

        public IQueryable<ModelBase> Source => null!;

        public bool UseVmSource => false;

        public bool Creatable => false;

        public IEnumerable<Type> ChildModels { get { yield break; } }

        public IEnumerable<Column> Columns { get { yield break; } }

        public ObservableListWrap<ModelBase>? VmSource(object parentVm, CrudViewModelBase? elementVm)
        {
            return null;
        }
    }
    internal class ObjectInputSplashDescription : InputSplashDescription, IObjectPropertyDescription
    {
        public bool Selectable => true;

        public BindingBase DisplayMemberBinding => null!;

        public string DisplayMemberPath => null!;

        public IQueryable<ModelBase> Source { get; }

        public bool UseVmSource => false;

        public bool Creatable => false;

        public IEnumerable<Type> ChildModels { get { yield break; } }

        public ObservableListWrap<ModelBase>? VmSource(object parentVm, CrudViewModelBase? elementVm)
        {
            return null;
        }

        public ObjectInputSplashDescription(Type model)
        {
            Source = Proteus.Infer(model)!.AllBase(model);
        }
    }
}