/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

namespace TheXDS.Proteus.Component
{
    /// <summary>
    /// Describe una serie de miembros a implementar por una clase que 
    /// acepte mensajes de eventos ocurridos dentro de la aplicación además de
    /// ofrecer ciertos servicios básicos de interactividad.
    /// </summary>
    public interface IInteractiveMessageTarget : IMessageTarget
    {
        /// <summary>
        /// Realiza una pregunta al usuario.
        /// </summary>
        /// <param name="title">Título del mensaje.</param>
        /// <returns>
        /// <see langword="true"/> si el usuario ha dicho que sí al cuadro
        /// de diálogo, <see langword="false"/> en caso contrario.
        /// </returns>
        bool Ask(string title)
        {
            return Ask(title, "¿Está seguro que desea realizar esta operación?");
        }

        /// <summary>
        /// Realiza una pregunta al usuario.
        /// </summary>
        /// <param name="title">Título del mensaje.</param>
        /// <param name="message">Contenido de la pregunta.</param>
        /// <returns>
        /// <see langword="true"/> si el usuario ha dicho que sí al cuadro
        /// de diálogo, <see langword="false"/> en caso contrario.
        /// </returns>
        bool Ask(string title, string message);
    }
}