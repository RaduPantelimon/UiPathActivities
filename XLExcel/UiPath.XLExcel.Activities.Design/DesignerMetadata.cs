using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace UiPathTeam.XLExcel.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();

            attributeTableBuilder.AddCustomAttributes(typeof(XLExcelApplicationScope), new DesignerAttribute(typeof(XLExcelApplicationScopeDesigner)));
            attributeTableBuilder.AddCustomAttributes(typeof(ReadRange), new DesignerAttribute(typeof(ReadRangeDesigner)));
            //attributeTableBuilder.AddCustomAttributes(typeof(ConvertXLSToXLSX), new DesignerAttribute(typeof(ConvertXLSToXLSXDesigner)));

            MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());
        }
    }
}
