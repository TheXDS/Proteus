/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

namespace TheXDS.Proteus.Dialogs
{
    /// <summary>
    /// Enumera el tipo de mensaje a mostrar.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Mensaje.
        /// </summary>
        Message,

        /// <summary>
        /// Mensaje informativo.
        /// </summary>
        Info,

        /// <summary>
        /// Pregunta.
        /// </summary>
        Question,

        /// <summary>
        /// Advertencia.
        /// </summary>
        Warning,

        /// <summary>
        /// Detención de operación.
        /// </summary>
        Stop,

        /// <summary>
        /// Error.
        /// </summary>
        Error,

        /// <summary>
        /// Crítico.
        /// </summary>
        Critical
    }
}