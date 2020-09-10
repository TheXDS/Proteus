/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System.Threading.Tasks;
using TheXDS.MCART.Security.Password;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.DevelModule.Seeders
{
    [SeederFor(typeof(UserService))]
    public class DevelUserSeeder : IAsyncDbSeeder
    {
        private static UserRole? _role;
        public async Task<DetailedResult> SeedAsync(IFullService service, IStatusReporter? reporter)
        {
            reporter?.UpdateStatus("Creando usuarios de desarrollo...");
            _role ??= await service.FirstOrDefaultAsync<UserRole>(p => p.DefaultGranted == SecurityFlags.Root);
            return await service.AddAsync(new[] {
                Rootified(NewDevelUser("devel", SecurityFlags.Root)),
                NewDevelUser("admin", SecurityFlags.FullAdmin),
                NewDevelUser("operator", SecurityFlags.ReadWrite),
                NewDevelUser("viewer", SecurityFlags.Read, SecurityBehavior.Visible),
                NewDevelUser("restricted", SecurityFlags.None, SecurityBehavior.Locked),
                NewDevelUser("user", SecurityFlags.None, null),
            });
        }

        public async Task<bool> ShouldRunAsync(IReadAsyncService service, IStatusReporter? reporter)
        {
            reporter?.UpdateStatus("Comprobando usuarios de desarrollo...");
            return !await service.AnyAsync<User>(p => p.Id == "devel");
        }

        private static User Rootified(User u)
        {
            if (!(_role is null))
            {
                u.Roles.Add(_role);
            }
            else
            {
                u.DefaultGranted = SecurityFlags.Root;
                u.DefaultRevoked = SecurityFlags.None;
                u.ButtonBehavior = SecurityBehavior.Unlocked;
                u.ModuleBehavior = SecurityBehavior.Unlocked;
            }
            return u;
        }

        private static User NewDevelUser(string id, SecurityFlags securityFlags, SecurityBehavior? visibility = SecurityBehavior.Enabled)
        {
            return new User()
            {
                Id = id,
                AllowMultiLogin = true,
                Enabled = true,
                Interactive = true,
                ScheduledPasswordChange = false,
                DefaultGranted = securityFlags,
                ButtonBehavior = visibility,
                ModuleBehavior = visibility,
                PasswordHash = PasswordStorage.CreateHash(id.ToSecureString()),
                Name = $"Desarrollador '{securityFlags.NameOf()}' de Proteus"
            };
        }
    }
}
