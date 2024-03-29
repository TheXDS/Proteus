﻿using TheXDS.Proteus.Controls.Base;
using TheXDS.Triton.Models.Base;
using static TheXDS.Ganymede.Helpers.DependencyObjectHelpers;

namespace TheXDS.Proteus.Controls;

/// <summary>
/// Controls that allows the user to manage <see cref="Model"/> properties.
/// </summary>
public class SingleObjectEditor : ObjectEditor
{
    static SingleObjectEditor()
    {
        SetControlStyle<SingleObjectEditor>(DefaultStyleKeyProperty);
    }
}