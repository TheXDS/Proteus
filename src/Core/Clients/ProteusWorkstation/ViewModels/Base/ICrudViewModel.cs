﻿/*
Copyright © 2017-2019 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TheXDS.Proteus.Models.Base;
using TheXDS.Proteus.Widgets;

namespace TheXDS.Proteus.ViewModels.Base
{
    /// <summary>
    ///     Define una serie de miembros a implementar por una clase que
    ///     administre las operaciones de Crud con elementos de UI
    ///     autogenerados.
    /// </summary>
    public interface ICrudViewModel : ICrudEditingViewModel
    {
        /// <summary>
        ///     Obtiene la ventana de detalles de la entidad seleccionada.
        /// </summary>
        FrameworkElement? SelectedDetails { get; }

        /// <summary>
        ///     Obtiene el editor a utlizar para editar a la entidad seleccionada.
        /// </summary>
        FrameworkElement? SelectedEditor { get; }

        /// <summary>
        ///     Comando para la creación de nuevas entidades.
        /// </summary>
        ICommand CreateNew { get; }

        /// <summary>
        ///     Comando para la edición de la entidad actualmente seleccionada.
        /// </summary>
        ICommand EditCurrent { get; }

        /// <summary>
        ///     Comando para la eliminación de la entidad actualmente
        ///     seleccionada.
        /// </summary>
        ICommand DeleteCurrent { get; }

        /// <summary>
        ///     Obtiene un valor de visibilidad aplicable mientras el ViewModel se encuentre ocupado.
        /// </summary>
        Visibility BusyV { get; }

        /// <summary>
        ///     Obtiene un valor de visibilidad aplicable mientras el ViewModel no se encuentre ovupado.
        /// </summary>
        Visibility NotBusyV { get; }

        /// <summary>
        ///     Obtiene un valor de visibilidad aplicable cuando el ViewModel administre la creación de múltiples modelos.
        /// </summary>
        Visibility MultiModel { get; }

        /// <summary>
        ///     Obtiene un valor de visibilidad aplicacble cuando el ViewModel administre la creación de un único modelo.
        /// </summary>
        Visibility UniModel { get; }

        /// <summary>
        ///     Obtiene un valor que indica si el ViewModel se encuentra actualmente en modo de edición.
        /// </summary>
        bool EditMode { get; }

        /// <summary>
        ///     Obtiene un valor que indica si el ViewModel no se encuentra actualmente en modo de edición.
        /// </summary>
        bool NotEditMode { get; }

        /// <summary>
        ///     Obtiene un valor que indica si el ViewModel no está ocupado.
        /// </summary>
        bool NotBusy { get; }

        /// <summary>
        ///     Obtiene un valor de visibilidad aplicable cuando el ViewModel se encuentre en modo de edición.
        /// </summary>
        Visibility EditVis { get; }

        /// <summary>
        ///     Obtiene un valor de visibilidad aplicable cuando el ViewModel no se encuentre en modo de edición.
        /// </summary>
        Visibility NotEditVis { get; }

        /// <summary>
        ///     Enumeración de comandos para la creación de entidades cuando
        ///     este ViewModel administra dos o más modelos de datos.
        /// </summary>
        IEnumerable<Launcher>? CreateCommands { get; }

        /// <summary>
        ///     Determina si es posible ejecutar el comando para la creación de
        ///     nuevas entidades.
        /// </summary>
        /// <param name="t">
        ///     Tipo de modelo.
        /// </param>
        /// <returns>
        ///     En su implementación predeterminada, este método siempre
        ///     devuelve <see langword="true"/>.
        /// </returns>
        bool CanCreate(Type t);

        /// <summary>
        ///     Determina si es posible editar a la entidad seleccionada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        ///     <see langword="true"/> si es posible editar la entidad
        ///     seleccionada, <see langword="false"/> en caso contrario.
        /// </returns>
        bool CanEdit(ModelBase entity);

        /// <summary>
        ///     Determina si es posible eliminar a la entidad seleccionada.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        ///     <see langword="true"/> si es posible eliminar la entidad
        ///     seleccionada, <see langword="false"/> en caso contrario.
        /// </returns>
        bool CanDelete(ModelBase entity);

    }
}