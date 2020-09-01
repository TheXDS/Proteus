/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Linq;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Crud
{
    public class UserRoleDescriptor : ProteusCredentialDescriptor<UserRole>
    {
        protected override void DescribeModel()
        {
            FriendlyName("Rol de usuario");

            base.DescribeModel();

            Property(u => u.Members)
                .Column("Entidad", (ProteusHierachicalCredential p) => p.Name)
                .AllowSelection()
                .ShowInDetails()
                .Label("Miembros");

            BeforeSave(SetId);
            CanDelete(Chk);
        }

        private bool Chk(UserRole arg)
        {
            return !arg.Members.Any();
        }

        private void SetId(UserRole obj)
        {
            if (obj.IsNew) obj.Id = Guid.NewGuid().ToString();
        }
    }
}