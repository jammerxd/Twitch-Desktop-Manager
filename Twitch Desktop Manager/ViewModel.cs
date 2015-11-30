using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
namespace Twitch_Desktop_Manager
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
        IMainWindowCallbacks mainWindowCallbacks = null;
        public string windowTitle { get; set; }

        public ViewModel(IMainWindowCallbacks mainCallbacks)
        {
            if (mainCallbacks == null)
            {
                throw new ArgumentException("mainwindow callbacks can't be null");
            }
            mainWindowCallbacks = mainCallbacks;
            windowTitle = "Twitch_Desktop_Manager";
        }
    }
}
