using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformsMVCAsyncExampleNetStandard.Controllers.Main
{
    public interface IView
    {
        void ModelChange(object sender, ModelChangeEventArgs e);
    }

}
