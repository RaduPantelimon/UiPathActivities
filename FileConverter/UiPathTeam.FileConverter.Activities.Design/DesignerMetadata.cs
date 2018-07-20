using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace UiPathTeam.FileConverter.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();
            attributeTableBuilder.AddCustomAttributes(typeof(ConvertXLSToXLSX), 
                new DesignerAttribute(typeof(ConvertFileDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(ConvertDOCToDOCX), 
                new DesignerAttribute(typeof(ConvertFileDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(ConvertDOCXToDOC), 
                new DesignerAttribute(typeof(ConvertFileDesigner)));

            attributeTableBuilder.AddCustomAttributes(typeof(ConvertToPDF),
                new DesignerAttribute(typeof(ConvertFileDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(ConvertXLSXToXLS),
                new DesignerAttribute(typeof(ConvertFileDesigner)));

            MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());
        }
    }
}
