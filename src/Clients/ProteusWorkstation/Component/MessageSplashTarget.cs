/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Component;
using System;

namespace TheXDS.Proteus.Dialogs
{
    /// <summary>
    /// <see cref="IMessageTarget"/> para mostrar mensajes por medio de una
    /// instancia de la clase <see cref="MessageSplash"/>.
    /// </summary>
    public class MessageSplashTarget : IInteractiveMessageTarget
    {
        /// <summary>
        /// Realiza una pregunta al usuario.
        /// </summary>
        /// <param name="title">Título del mensaje.</param>
        /// <param name="message">Contenido de la pregunta.</param>
        /// <returns>
        /// <see langword="true"/> si el usuario ha dicho que sí al cuadro
        /// de diálogo, <see langword="false"/> en caso contrario.
        /// </returns>
        public bool Ask(string title, string message) => MessageSplash.Ask(title, message);

        /// <summary>
        /// Realiza una pregunta al usuario.
        /// </summary>
        /// <param name="title">Título del mensaje.</param>
        /// <returns>
        /// <see langword="true"/> si el usuario ha dicho que sí al cuadro
        /// de diálogo, <see langword="false"/> en caso contrario.
        /// </returns>
        public bool Ask(string title) => MessageSplash.Ask(title);

        /// <summary>
        /// Notifica de un error crítico.
        /// </summary>
        /// <param name="message">
        /// Mensaje a enviar al objetivo.
        /// </param>
        public void Critical(string message)
        {
            MessageSplash.Show("Error crítico", message, MessageType.Critical);
        }

        /// <summary>
        /// Notifica de un error crítico.
        /// </summary>
        /// <param name="ex">
        /// Excepción producida a notificar.
        /// </param>
        public void Critical(Exception ex)
        {
            MessageSplash.Show("Error crítico", ex.Message, MessageType.Critical);
        }
        
        /// <summary>
        /// Notifica de un mensaje de error.
        /// </summary>
        /// <param name="message">
        /// Mensaje a enviar al objetivo.
        /// </param>
        public void Error(string message)
        {
            MessageSplash.Show("Error", message, MessageType.Error);
        }

        /// <summary>
        /// Notifica de un mensaje informativo.
        /// </summary>
        /// <param name="message">
        /// Mensaje a enviar al objetivo.
        /// </param>
        public void Info(string message)
        {
            MessageSplash.Show("Información", message, MessageType.Info);
        }

        /// <summary>
        /// Notifica de un mensaje simple.
        /// </summary>
        /// <param name="message">
        /// Mensaje a enviar al objetivo.
        /// </param>
        public void Show(string message)
        {
            MessageSplash.Show("Mensaje", message, MessageType.Message);
        }

        /// <summary>
        /// Notifica de un mensaje simple.
        /// </summary>
        /// <param name="title">
        /// Título del mensaje.
        /// </param>
        /// <param name="message">
        /// Mensaje a enviar al objetivo.
        /// </param>
        public void Show(string title, string message)
        {
            MessageSplash.Show(title, message, MessageType.Message);
        }

        /// <summary>
        /// Notifica de un mensaje producido por una operación que debe
        /// detenerse.
        /// </summary>
        /// <param name="message">
        /// Mensaje a enviar al objetivo.
        /// </param>
        public void Stop(string message)
        {
            MessageSplash.Show("Alto", message, MessageType.Stop);
        }

        /// <summary>
        /// Notifica de una advertencia.
        /// </summary>
        /// <param name="message">
        /// Mensaje a enviar al objetivo.
        /// </param>
        public void Warning(string message)
        {
            MessageSplash.Show("Advertencia", message, MessageType.Warning);
        }
    }
}