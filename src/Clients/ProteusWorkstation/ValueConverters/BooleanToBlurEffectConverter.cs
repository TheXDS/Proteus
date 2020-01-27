﻿/*
Copyright © 2017-2019 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace TheXDS.Proteus.ValueConverters
{
    public class BooleanToBlurEffectConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b) return new BlurEffect { Radius = 5 };
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is BlurEffect;
        }
    }
}