using System.Activities.Presentation.Metadata;
using System.ComponentModel;

using UiPathTeam.WpfFormCreator.Activities;

namespace UiPathTeam.WpfFormCreator.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();

            attributeTableBuilder.AddCustomAttributes(typeof(XamlFormCreator), new DesignerAttribute(typeof(XamlFormCreatorDesigner)));


            MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());
        }
    }
}
