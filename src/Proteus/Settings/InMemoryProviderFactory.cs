/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TheXDS.Proteus.Component
{
    public class InMemoryProviderFactory : IDbConnectionFactory
    {
        private static bool _registered;
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            return Effort.DbConnectionFactory.CreatePersistent($"Proteus_{Guid.NewGuid()}");
        }

        public InMemoryProviderFactory()
        {
            if (!_registered)
            {
                _registered = true;
                Effort.Provider.EffortProviderConfiguration.RegisterProvider();
            }
        }
    }
}