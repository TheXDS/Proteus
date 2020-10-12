/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Windows;
using System.Windows.Controls;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Pages
{
    public class SettingCellTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var t = ((Setting)item).DataType;

            if (t.IsEnum)
            {
                return Application.Current.TryFindResource($"EnumSettingsControl") as DataTemplate 
                    ?? base.SelectTemplate(item, container);
            }
            else
            {
                return Application.Current.TryFindResource($"{t.Name}SettingsControl") as DataTemplate
                    ?? Application.Current.TryFindResource($"StringSettingsControl") as DataTemplate
                    ?? base.SelectTemplate(item, container);
            }
        }
    }
}