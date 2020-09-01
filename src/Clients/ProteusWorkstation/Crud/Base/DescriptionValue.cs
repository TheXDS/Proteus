/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Component.Attributes;

namespace TheXDS.Proteus.Crud.Base
{
    /// <summary>
    /// Enumeración que permite definir un tipo de valor a almacenar o 
    /// recuperar para la configuración de Crud de una propiedad.
    /// </summary>
    public enum DescriptionValue : int
    {
        /// <summary>
        /// Obtener o establecer el valor predetemrinado de la propiedad.
        /// </summary>
        Default,
        /// <summary>
        /// Valor que indica si se debe utilizar un valor predetermiando
        /// configurado para la propiedad.
        /// </summary>
        UseDefault,
        /// <summary>
        /// Etiqueta del campo generado para la propiedad.
        /// </summary>
        Label,
        /// <summary>
        /// Ícono del campo generado para la propiedad.
        /// </summary>
        Icon,
        /// <summary>
        /// Formato a utilizar para la propiedad.
        /// </summary>
        Format,
        /// <summary>
        /// Valor que define la visibilidad de un campo en el editor de Crud.
        /// </summary>
        [DescriptionDefault(true)] Visible,
        /// <summary>
        /// Valor que indica si un campo es o no de sólo lectura.
        /// </summary>
        ReadOnly,
        /// <summary>
        /// Valor de nulabilidad del campo.
        /// </summary>
        [DescriptionDefault(NullMode.Infer)] Nullability,
        /// <summary>
        /// Grupo de selección de Radio.
        /// </summary>
        RadioGroup,
        /// <summary>
        /// Orden de generación del campo.
        /// </summary>
        Order,
        /// <summary>
        /// Colección de funciones de validación.
        /// </summary>
        Validations,
        /// <summary>
        /// Ayuda emergente del campo.
        /// </summary>
        Tooltip,
        /// <summary>
        /// Visibilidad de marca de agua (etiqueta) del campo.
        /// </summary>
        [DescriptionDefault(true)] WatermarkVisibility,
        /// <summary>
        /// Valor que indica si se debe o no mostrar el campo generado como una
        /// columna de la tabla de vista.
        /// </summary>
        [DescriptionDefault(false)] AsListColumn,
        /// <summary>
        /// Valor que indica si el campo debe ser incluido o no en la vista de
        /// detalles del Crud.
        /// </summary>
        [DescriptionDefault(false)] ShowInDetails,
        /// <summary>
        /// Tipo de campo de texto a generar para la propiedad.
        /// </summary>
        [DescriptionDefault(Base.TextKindEnum.Generic)] TextKind,
        /// <summary>
        /// Metadatos del campo de texto a generar para la propiedad.
        /// </summary>
        TextKindMetadata,

        /// <summary>
        /// Valor de rango numérico para la propiedad.
        /// </summary>
        Range,

        /// <summary>
        /// Valor que indica si el campo de fecha debe incluir un editor de
        /// hora.
        /// </summary>
        WithTime,

        /// <summary>
        /// Valor que indica si el editor de Crud debe permitir crear entidades
        /// nuevas.
        /// </summary>
        Creatable,
        /// <summary>
        /// Valor que indica si el editor de Crud debe permitir seleccionar
        /// entidades existentes desde una lista.
        /// </summary>
        Selectable,
        /// <summary>
        /// Valor que indica si el editor de Crud debe permitir la edición de
        /// entidades existentes.
        /// </summary>
        Editable,
        /// <summary>
        /// Valor que indica una ruta a la propiedad a utilizar para desplegar
        /// el valor de la entidad.
        /// </summary>
        DisplayMember,
        /// <summary>
        /// Origen de datos del selector de lista de la entidad.
        /// </summary>
        ListSource,
        ListSourceType,
        /// <summary>
        /// Bindings personalizados a aplicar directamente a los controles del
        /// Crud generado.
        /// </summary>
        Bindings,
        /// <summary>
        /// Columnas de una lista personalizada.
        /// </summary>
        ListColumns
    }

    public enum ListSourceTypeEnum
    {
        Queryable,
        Collection,
        FuncEnumerable,        
    }
}