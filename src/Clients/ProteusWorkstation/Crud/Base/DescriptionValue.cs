/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using TheXDS.Proteus.Component.Attributes;

namespace TheXDS.Proteus.Crud.Base
{
    public enum DescriptionValue
    {
        Default,
        Label,
        Icon,
        Format,
        [DescriptionDefault(true)] Visible,
        ReadOnly,
        [DescriptionDefault(NullMode.Infer)] Nullability,
        RadioGroup,
        Order,
        Validations,
        Tooltip,
        [DescriptionDefault(true)] WatermarkVisibility,
        [DescriptionDefault(false)] AsListColumn,
        [DescriptionDefault(false)] ShowInDetails,


        [DescriptionDefault(Base.TextKind.Generic)] TextKind,
        TextKindMetadata,

    }


}