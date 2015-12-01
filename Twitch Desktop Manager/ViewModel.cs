using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Reflection;
using System.Diagnostics;
namespace Twitch_Desktop_Manager
{
    [ImplementPropertyChanged]
    public class ViewModel
    {
        IMainWindowCallbacks mainWindowCallbacks = null;


        //--------------VARIABLES--------------\\
        #region Variables
        public string windowTitle { get; set; }
        public string version { get; set; }
        #endregion
        //--------------END OF VARIABLES--------------\\

        public ViewModel(IMainWindowCallbacks mainCallbacks)
        {
            if (mainCallbacks == null)
            {
                throw new ArgumentException("mainwindow callbacks can't be null");
            }
            mainWindowCallbacks = mainCallbacks;
            version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
            windowTitle = "Twitch Desktop Manager v" + version;
        }
    }
}
