/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Api;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Crud
{

    public class UserDescriptor: ProteusCredentialDescriptor<User>
    {
        protected override void DescribeModel()
        {
            FriendlyName("Usuario");

            Property(u => u.Id).Id("Login", "🔑");
            Property(p => p.PasswordHash).Label("Contraseña").Required();

            base.DescribeModel();

            Property(u => u.Parent)
                .Label("Grupo padre")
                .Required()
                .ShowInDetails()
                .Selectable()
                .AsListColumn();

            Property(u => u.Roles)
                .Label("Roles de usuario")
                .AllowSelection()
                .ShowInDetails();

            Property(u => u.Enabled)
                .Label("Usuario activo")
                .ShowInDetails()
                .AsListColumn()
                .Default(true);

            Property(u => u.Interactive)
                .Label("Usuario interactivo")
                .ShowInDetails()
                .Default(true);

            Property(p => p.ScheduledPasswordChange)
                .Label("Cambiar contraseña en el siguiente inicio de sesión")
                .ShowInDetails();

            Property(p => p.AllowMultiLogin)
                .Label("Permitir múltiples sesiones")
                .ShowInDetails();

            CustomAction("Cambiar contraseña...", ChPassword);
            CanDelete(u => u.Interactive && u.DefaultGranted != SecurityFlags.Root);
        }

        private void ChPassword(User obj)
        {
            if (MCART.Dialogs.PasswordDialog.ConfirmPassword(out var data))
            {
                obj.PasswordHash = MCART.Security.Password.PasswordStorage.CreateHash(data.Password);
            }
        }
    }
}