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
                NewDevelUser("viewer", SecurityFlags.Read),
                NewDevelUser("restricted", SecurityFlags.None)
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
                u.ButtonBehavior = Models.Base.SecurityBehavior.Unlocked;
                u.ModuleBehavior = Models.Base.SecurityBehavior.Unlocked;
            }
            return u;
        }

        private static User NewDevelUser(string id, SecurityFlags securityFlags)
        {
            return new User()
            {
                Id = id,
                AllowMultiLogin = true,
                Enabled = true,
                Interactive = true,
                ScheduledPasswordChange = false,
                DefaultGranted = securityFlags,
                PasswordHash = PasswordStorage.CreateHash(id.ToSecureString()),
                Name = $"Desarrollador '{securityFlags.NameOf()}' de Proteus"
            };
        }
    }
}
