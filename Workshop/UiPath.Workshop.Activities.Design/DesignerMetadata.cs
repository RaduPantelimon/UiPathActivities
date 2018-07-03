using System.Activities.Presentation.Metadata;

namespace UiPathTeam.Workshop.Activities.Design
{
    class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();

            MetadataStore.AddAttributeTable(attributeTableBuilder.CreateTable());
        }
    }
}
