using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace UiPathTeam.FileConverter.Activities.Design
{
    class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();
            attributeTableBuilder.AddCustomAttributes(typeof(ConvertXLSToXLSX), new DesignerAttribute(typeof(ConvertXLSToXLSXDesigner)));

            MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());
        }
    }
}
