﻿/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Models.Base;
using TheXDS.MCART.Types;

namespace TheXDS.Proteus.Component
{
    /// <summary>
    /// Implementa un <see cref="ModelSearchFilter{T}"/> que puede filtrar
    /// entidades que implementen la interfaz <see cref="INameable"/>.
    /// </summary>
    public class UserIdFilter : SimpleModelSearchFilter<IUserBase>
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="UserIdFilter"/>.
        /// </summary>
        public UserIdFilter() : base(nameof(IUserBase.UserId)) { }
    }
}