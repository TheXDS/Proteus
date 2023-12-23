using System;
using System.Windows;
using System.Windows.Markup;
using TheXDS.Proteus.Component;

namespace TheXDS.Proteus.Markup;

/// <summary>
/// Implements a markup extension that returns a
/// <see cref="ProteusStackVisualResolver"/> adapted to resolve visuals of type
/// <see cref="FrameworkElement"/>.
/// </summary>
public class ProteusCrudBuilder : MarkupExtension
{
    /// <inheritdoc/>
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return new ProteusStackVisualResolver();
    }
}
