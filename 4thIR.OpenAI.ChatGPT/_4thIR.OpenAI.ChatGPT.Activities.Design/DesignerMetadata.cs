using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.OpenAI.ChatGPT.Activities.Design.Designers;
using _4thIR.OpenAI.ChatGPT.Activities.Design.Properties;

namespace _4thIR.OpenAI.ChatGPT.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(GenerateCompletion), categoryAttribute);
            builder.AddCustomAttributes(typeof(GenerateCompletion), new DesignerAttribute(typeof(GenerateCompletionDesigner)));
            builder.AddCustomAttributes(typeof(GenerateCompletion), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
