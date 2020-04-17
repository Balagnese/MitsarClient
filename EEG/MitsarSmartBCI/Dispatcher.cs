using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEG.MitsarSmartBCI
{
    public static class Dispatcher
    {
        public delegate void AsyncAction();

        public delegate void DispatcherInvoker(UserControl form, AsyncAction a);
        public static void Invoke(UserControl form, AsyncAction action)
        {
            if (!form.InvokeRequired)
            {
                action();
            }
            else
            {
                form.Invoke((DispatcherInvoker)Invoke, form, action);
            }
        }
    }
}
