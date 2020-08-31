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
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            return Effort.DbConnectionFactory.CreatePersistent($"Proteus_{Guid.NewGuid()}");
        }
    }
}