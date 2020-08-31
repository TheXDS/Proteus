/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Collections.Generic;
using TheXDS.MCART.Security.Password;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Seeders
{
    [SeederFor(typeof(UserService))]
    public class DemoUserSeeder : AsyncDbSeeder<User>
    {
        protected override IEnumerable<User> GenerateEntities()
        {
            yield return new User
            {
                Id = "demo",
                AllowMultiLogin = true,
                ButtonBehavior = Models.Base.SecurityBehavior.Unlocked,
                DefaultGranted = SecurityFlags.Root,
                DefaultRevoked = SecurityFlags.None,
                Descriptors = { },
                Enabled = true,
                Interactive = true,
                IsDeleted = false,
                ModuleBehavior = Models.Base.SecurityBehavior.Unlocked,
                Name = "Usuario de demostración",
                ScheduledPasswordChange = false,
                Parent = null!,
                Roles = { },
                PasswordHash = PasswordStorage.CreateHash("demo".ToSecureString())
            };
        }
    }
}