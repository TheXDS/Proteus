using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TheXDS.Proteus.ContabilidadUi.ViewModels;
using TheXDS.Proteus.Crud;
using TheXDS.Proteus.Crud.Base;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;
using static TheXDS.Proteus.Annotations.InteractionType;

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
    /// <see cref="Entidad"/>.
    /// </summary>
    public class EntidadDescriptor : CrudDescriptor<Entidad>
    {
        protected override void DescribeModel()
        {
            OnModuleMenu(AdminTool);

            Property(p => p.Name).AsName().AsListColumn();
            ListProperty(p => p.Periodos).Creatable().Important("Periodos contables");
            //ObjectProperty(p => p.Activo).Creatable().Important("Cuenta de activo").Required();
            //ObjectProperty(p => p.Pasivo).Creatable().Important("Cuenta de pasivo").Required();
            //ObjectProperty(p => p.Capital).Creatable().Important("Cuenta de patrimonio").Required();
            ShowAllInDetails();
            BeforeSave(CreateRoot);
        }

        private void CreateRoot(Entidad arg1, ModelBase arg2)
        {
            arg1.Activo = new Cuenta() { Name = "Activo" };
            arg1.Pasivo = new Cuenta() { Name = "Pasivo" };
            arg1.Capital = new Cuenta() { Name = "Patrimonio" };
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
            ObjectProperty(p => p.Activo).Creatable().Important("Cuenta de activo").Required();
            ObjectProperty(p => p.Pasivo).Creatable().Important("Cuenta de pasivo").Required();
            ObjectProperty(p => p.Capital).Creatable().Important("Cuenta de capital").Required();

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
            ObjectProperty(p => p.Cuenta).Selectable().Important("Cuenta afectada").Required().AsListColumn();
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
    public class PartidaDescriptor : CrudDescriptor<Partida>
    {
        protected override void DescribeModel()
        {
            DateProperty(p => p.Timestamp).WithTime().Important("Fecha/hora de partida").Default(DateTime.Now);
            Property(p => p.Description).Important("Descripción").NotEmpty().Required();
            ListProperty(p => p.Movimientos).Creatable().Important("Movimientos");
            ListProperty(p => p.Documentos).Creatable().Important("Documentos de referencia");
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
            ListProperty(p => p.Movimientos).Creatable().Label("Movimientos de la partida").Required().ShowInDetails();
        }
    }
}