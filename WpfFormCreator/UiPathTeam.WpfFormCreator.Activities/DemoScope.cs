using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.Workshop.Activities
{
    public class DemoScope:NativeActivity
    {

        public ActivityAction Body{ get; set; }

        public DemoScope()
        {
            Body = new ActivityAction();
            Body.Handler = new Sequence();
        }
        protected override void Execute(NativeActivityContext context)
        {
            
        }
    }
}
