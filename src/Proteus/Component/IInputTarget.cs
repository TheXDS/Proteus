/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

namespace TheXDS.Proteus.Component
{
    public interface IInputTarget
    {
        bool GetNew<T>(string prompt, out T value);
        bool Get<T>(string prompt, ref T value);
    }
}