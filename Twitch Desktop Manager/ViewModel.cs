using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using ChatSharp;
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

        public Boolean startupVisible { get; set; }
        public Boolean managerVisible { get; set; }

        public BackgroundWorker chatWorker { get; set; } //Handles server connection
        
        public string ircServer { get; set; }
        public string ircPort { get; set; }
        public string ircOATH { get; set; }
        public string ircUsername { get; set; }

        public string btnLoginText { get; set; }
        public Boolean saveLogin { get; set; }
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

            mainWindowCallbacks.ChangeDimensions(300, 300);
            btnLoginText = "Login";
            saveLogin = false;
            startupVisible = true;
            managerVisible = false;
            
            windowTitle = "Twitch Desktop Manager v" + version;

            chatWorker = new BackgroundWorker();
            chatWorker.DoWork += ChatWorker_DoWork;
            chatWorker.WorkerSupportsCancellation = true;
        }

        #region Relay Command Functions
        private void Login(object currentObject)
        {
            
        }
        #endregion

        #region Relay Commands
        private RelayCommand _LoginCommand { get; set; }
        public RelayCommand LoginCommand 
        {
            get 
            {
                if(_LoginCommand == null)
                {
                    _LoginCommand = new RelayCommand(Login);
                }
                return _LoginCommand;
            }
        }
        #endregion


        private void ChatWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }
    }
}
