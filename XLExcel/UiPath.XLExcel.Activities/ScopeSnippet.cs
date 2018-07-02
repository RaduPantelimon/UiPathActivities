using System.Activities;
using System.ComponentModel;

namespace UiPath.XLExcel.Activities
{
    class ScopeSnippet
    {
        void Snippet()
        {
            CodeActivityContext context = null;

            PropertyDescriptor property = context.DataContext.GetProperties()["PropertyName"];
            string value = property?.GetValue(context.DataContext) as string;
        }
    }
}
