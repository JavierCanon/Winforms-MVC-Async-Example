using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformsMVCAsyncExampleNetStandard.Controllers.Main
{

    public class ModelChangeEventArgs
    {
        public long ElapsedMilliseconds;

        private ModelChangeEventArgs() { }
        public ModelChangeEventArgs(long ElapsedMilliseconds)
        {
            this.ElapsedMilliseconds = ElapsedMilliseconds;
        }
    }

}
