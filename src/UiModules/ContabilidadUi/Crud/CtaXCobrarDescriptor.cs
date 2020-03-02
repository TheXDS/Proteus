﻿using System;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CtaXCobrar"/>.
    /// </summary>
    public class CtaXCobrarDescriptor : CrudDescriptor<CtaXCobrar, CtaXCobrarViewModel>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CtaXCobrar"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Annotations.InteractionType.AdminTool);
            FriendlyName("Cuenta por cobrar");
            ObjectProperty(p => p.Cliente).Selectable().Required().Important();
            Property(p => p.Timestamp).Important("Fecha de emisión").Default(DateTime.Today);
            VmObjectProperty(p => p.Kind).Selectable().Required().Label("Tipo de documento");
            Property(p => p.RefNum).Required().Important("# de ref. de factura/cuenta");
            NumericProperty(p => p.Total).Positive().Important("Total a cobrar");
            Property(p => p.Paid).Important("Cobrada").Hidden();

            VmBeforeSave(BuildPartida);
            AfterSave(async () => await ContabilidadModule.ModuleStatus.InitViewModel());
        }

        private void BuildPartida(CtaXCobrarViewModel arg1, ModelBase arg2)
        {
            var cta = arg1.Entity;
            if (cta.CreationPartida is { }) return;
            cta.Empresa = ContabilidadModule.ModuleStatus.ActiveEmpresa!;
            var current = Proteus.Service<ContabilidadService>()!.First<ClienteXEmpresa>(p => p.Empresa.Id == cta.Empresa!.Id);

            cta.CreationPartida = new Partida
            {
                Description = $"{arg1.Kind.Name} por cobrar de Cliente {cta.Cliente} por {cta.Total:C}, número de referencia {cta.RefNum}",
                Entidad = ContabilidadModule.ModuleStatus.ActiveEntidad,
                Documentos =
                {
                    new DocumentRef
                    {
                        Kind = arg1.Kind,
                        DocReference = cta.RefNum
                    }
                },
                Timestamp = DateTime.Now,
                Parent = ContabilidadModule.ModuleStatus.ActivePeriodo!,
                Movimientos =
                {
                    new Movimiento
                    {
                        RawValue = -cta.Total,
                        Cuenta = current.IngresoCuenta
                    },
                    new Movimiento
                    {
                        RawValue = cta.Total,
                        Cuenta = current.DebitoCuenta
                    }
                }
            };
        }
    }
}