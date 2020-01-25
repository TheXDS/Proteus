using System;
using System.Collections.Generic;
using System.Reflection;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using System.Windows;
using TheXDS.Proteus.Models.Base;
using static TheXDS.Proteus.Annotations.InteractionType;
using System.Linq;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.ContabilidadUi.Modules;

namespace TheXDS.Proteus.ContabilidadUi.Crud
{
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Cuenta"/>.
    /// </summary>
    public class CuentaDescriptor : CrudDescriptor<Cuenta>
    {
        /// <summary>
        /// Describe el modelo <see cref="Cuenta"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            Property(p => p.Name).AsName().AsListColumn();
            ListProperty(p => p.Children).Creatable().Important("Cuentas hijas");
            ListProperty(p => p.SubCuentas).Creatable().Important("Sub-cuentas");
            ShowAllInDetails();
            Template();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CuentaMolde"/>.
    /// </summary>
    public class CuentaMoldeDescriptor : CrudDescriptor<CuentaMolde>
    {
        protected override void DescribeModel()
        {
            Property(p => p.Name).AsName().AsListColumn();
            ListProperty(p => p.Children).Creatable().Important("Cuentas hijas");

            ShowAllInDetails();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Empresa"/>.
    /// </summary>
    public class EmpresaDescriptor : CrudDescriptor<Empresa, EmpresaViewModel>
    {
        protected override void DescribeModel()
        {
            OnModuleMenu(AdminTool);

            Property(p => p.Name).AsName().AsListColumn();
            this.DescribeAddress();
            this.DescribeContact();
            ListProperty(p => p.Periodos).Creatable().Important("Periodos contables");
            ShowAllInDetails();

            VmObjectProperty(p => p.FromMolde).Selectable()
                .Bind(UIElement.IsEnabledProperty, nameof(EmpresaViewModel.CanAddMolde))
                .Nullable()
                .Label("Crear árbol contable desde plantilla");

            VmBeforeSave(CreateRoot);
            CustomAction("Abrir nuevo período", NewPeriod);
            AfterSave(UpdateViewModel);
        }

        private async void UpdateViewModel(Empresa arg1, ModelBase arg2)
        {            
            await ContabilidadModule.ModuleStatus.InitViewModel();
        }

        private void CreateRoot(EmpresaViewModel arg1, ModelBase arg2)
        {
            if (arg1.FromMolde is { } m)
            {
                arg1.Entity.Activo = m.Activo;
                arg1.Entity.Pasivo = m.Pasivo;
                arg1.Entity.Patrimonio = m.Patrimonio;
            }
            else
            {
                arg1.Entity.Activo = new Cuenta() { Name = "Activo" };
                arg1.Entity.Pasivo = new Cuenta() { Name = "Pasivo" };
                arg1.Entity.Patrimonio = new Cuenta() { Name = "Patrimonio" };
            }
        }

        private void NewPeriod(Empresa obj)
        {
            
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Molde"/>.
    /// </summary>
    public class MoldeDescriptor : CrudDescriptor<Molde>
    {
        protected override void DescribeModel()
        {
            OnModuleMenu(Settings);
            FriendlyName("Molde de árbol contable");

            Property(p => p.Name).AsName().AsListColumn();
            ObjectProperty(p => p.Activo).Creatable().Important("Molde de cuenta de activo").Required();
            ObjectProperty(p => p.Pasivo).Creatable().Important("Molde de cuenta de pasivo").Required();
            ObjectProperty(p => p.Patrimonio).Creatable().Important("Molde de cuenta de capital").Required();

            ShowAllInDetails();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Movimiento"/>.
    /// </summary>
    public class MovimientoDescriptor : CrudDescriptor<Movimiento, MovimientoViewModel>
    {
        protected override void DescribeModel()
        {
            ObjectProperty(p => p.Cuenta)
                .Selectable()
                .Important("Cuenta afectada")
                .Required()
                .AsListColumn();
            ObjectProperty(p => p.CostCenter).Selectable().Nullable().Label("Centro de costo afectado").AsListColumn().ShowInDetails();
            Property(p => p.RawValue).Important("Valor del movimiento").Validator<Movimiento>(CheckNotZero);
            VmProperty(p => p.Debe).Important("Valor del Debe");
            VmProperty(p => p.Haber).Important("Valor del Haber");
            VmProperty(p => p.RealValue).Label("Valor real").OnlyInDetails();
            VmProperty(p => p.LocalValue).Label("Valor en moneda local").OnlyInDetails();
            Property(p => p.ExchangeRate).Nullable().Label("Tasa de cambio").ShowInDetails();
        }

        private IEnumerable<ValidationError> CheckNotZero(Movimiento arg1, PropertyInfo arg2)
        {
            if (arg1.RawValue == 0m) yield return new ValidationError(arg2, "El movimiento debe tener un valor");
        }
    }
    
    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Partida"/>.
    /// </summary>
    public class PartidaDescriptor : CrudDescriptor<Partida, PartidaViewModel>
    {
        protected override void DescribeModel()
        {
            OnModuleMenu(Operation);

            DateProperty(p => p.Timestamp).WithTime().Important("Fecha/hora de partida").Default(DateTime.Now);
            ObjectProperty(p => p.Entidad)
                .Selectable()
                .Nullable()
                .Important("Entidad afectada");

            Property(p => p.Description).Important("Descripción").NotEmpty().Required();
            ListProperty(p => p.Movimientos).Creatable().Important("Movimientos").Validator<Partida>(ChkCuadrada);
            VmProperty(p => p.Cuadre).Label("Valor de cuadre").ReadOnly();
            ListProperty(p => p.Documentos).Creatable().Important("Documentos de referencia");

            BeforeSave(SetPeriod);
        }

        private void SetPeriod(Partida partida, ModelBase arg2)
        {
            if (arg2 is Periodo)
            {
                
            }
            else
            {
                partida.Parent = ContabilidadModule.ModuleStatus.ActivePeriodo;
            }
        }

        private IEnumerable<ValidationError> ChkCuadrada(Partida partida, PropertyInfo prop)
        {
            var m = partida.Movimientos;
            if (!m.Any()) yield return new NullValidationError(prop);
            if (m.Sum(p => p.RawValue) != 0m) yield return new ValidationError(prop,"Los movimientos no cuadran.");
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="DocumentKind"/>.
    /// </summary>
    public class DocumentKindDescriptor : CrudDescriptor<DocumentKind>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="DocumentKind"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Settings | Essential);
            Property(p => p.Name).AsName();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="CostCenter"/>.
    /// </summary>
    public class CostCenterDescriptor : CrudDescriptor<CostCenter>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="CostCenter"/>.
        /// </summary>
        protected override void DescribeModel()
        {            
            FriendlyName("Centro de costo");
            Property(p => p.Name).AsName();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Entidad"/>.
    /// </summary>
    public class EntidadDescriptor : CrudDescriptor<Entidad>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="Entidad"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(AdminTool);
            Property(p => p.Name).AsName();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="DocumentRef"/>.
    /// </summary>
    public class DocumentRefDescriptor : CrudDescriptor<DocumentRef>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="DocumentRef"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            ObjectProperty(p => p.Kind).Important("Tipo de documento").Required();
            Property(p => p.DocReference).Important("Número de referencia").NotEmpty();
            TextProperty(p => p.FilePath)
                .TextKind(TextKind.FilePath)
                .Nullable()
                .Label("Archivo de referencia");
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="Periodo"/>.
    /// </summary>
    public class PeriodoDescriptor : CrudDescriptor<Periodo>
    {
        protected override void DescribeModel()
        {
            DateProperty(p => p.Timestamp).Important("Fecha de inicio de periodo").Default(new DateTime(DateTime.Today.Year, 1, 1));
            DateProperty(p => p.Void).Nullable().Label("Fecha de cierre de periodo").AsListColumn().ShowInDetails();//.Default(new DateTime(DateTime.Today.Year, 12, 31));
            ListProperty(p => p.Partidas).Creatable().Label("Partidas del período").ShowInDetails().Required();
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="SubCuenta"/>.
    /// </summary>
    public class SubCuentaDescriptor : CrudDescriptor<SubCuenta>
    {
        protected override void DescribeModel()
        {
            Property(p => p.Name).AsName();
            //ListProperty(p => p.Movimientos).Creatable().Label("Movimientos de la partida").Required().ShowInDetails();
            Template();
        }
    }


    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="AccessControlList"/>.
    /// </summary>
    public class AccessControlListDescriptor : CrudDescriptor<AccessControlList>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="AccessControlList"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            OnModuleMenu(Settings);
            FriendlyName("Controles de acceso");

            LinkProperty<User>(p => p.UserId).Required().Important("Usuario");
            Property(p => p.EmpresaDefault).Nullable().Important("Acceso predeterminado para empresas");
            Property(p => p.EntidadDefault).Nullable().Important("Acceso predeterminado para entidades");
            Property(p => p.CostCenterDefault).Nullable().Important("Acceso predeterminado para centros de costo");

            ListProperty(p => p.Entries).Creatable().Required().Label("Accesos específicos");
        }
    }

    /// <summary>
    /// Describe las propiedades Crud para el modelo
    /// <see cref="AclEntry"/>.
    /// </summary>
    public class AclEntryDescriptor : CrudDescriptor<AclEntry>
    {
        /// <summary>
        /// Describe las propiedades Crud para el modelo
        /// <see cref="AclEntry"/>.
        /// </summary>
        protected override void DescribeModel()
        {
            FriendlyName("Control de acceso");

            ObjectProperty(p => p.Empresa).Selectable().RadioSelectable().Label("Empresa");
            ObjectProperty(p => p.Entidad).Selectable().RadioSelectable().Label("Entidad");
            ObjectProperty(p => p.CostCenter).Selectable().RadioSelectable().Label("Centro de costos");

            Property(p => p.Value).Required().Important("Acceso").Icon("🔑");
        }
    }

}