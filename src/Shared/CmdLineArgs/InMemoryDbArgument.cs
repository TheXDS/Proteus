/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.MCART.Component;
using TheXDS.Proteus.Component;

namespace TheXDS.Proteus.Cmd
{
    public class InMemoryArgument : Argument
    {
        public override string Summary => "Obliga a Proteus a utilizar una base de datos en memoria";
        public override void Run(CmdLineParser args)
        {
            DbConfig._forceInMemory = true;
        }
    }
}