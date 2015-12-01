using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.IO;
using TechLifeForum;
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

        public Boolean ircServerEnabled { get; set; }
        public Boolean ircPortEnabled { get; set; }
        public Boolean ircOATHEnabled { get; set; }
        public Boolean ircUsernameEnabled { get; set; }
        public Boolean saveLoginEnabled { get; set; }
        public Boolean btnLoginEnabled { get; set; }

        public Boolean progressBarVisible { get; set; }

        private Configuration configuration { get; set; }

        private IrcClient client { get; set; }
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

            configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            mainWindowCallbacks.ChangeDimensions(300, 300);
            btnLoginText = "Login";
            saveLogin = false;
            startupVisible = true;
            ircServerEnabled = true;
            ircPortEnabled = true;
            ircUsernameEnabled = true;
            ircOATHEnabled = true;
            saveLoginEnabled = true;
            btnLoginEnabled = true;
            progressBarVisible = false;
            managerVisible = false;
            
            windowTitle = "Twitch Desktop Manager v" + version;

            chatWorker = new BackgroundWorker();
            chatWorker.DoWork += ChatWorker_DoWork;
            chatWorker.WorkerSupportsCancellation = true;

            ircServer = GetSetting("url");
            ircPort = GetSetting("port");
            ircUsername = GetSetting("username");
            if (ircUsername != "")
            {
                saveLogin = true;
                ircOATH = Encryption.Decrypt(GetSetting("oath"), Encryption.GetHashString(ircUsername));
            }
        }


        #region Settings Functions
        private void AddorUpdateSetting(string key, string value = "")
        {
            if(configuration.AppSettings.Settings[key] == null)
            {
                configuration.AppSettings.Settings.Add(key, value);
            }
            else
            {
                configuration.AppSettings.Settings[key].Value = value;
            }
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private string GetSetting(string key)
        {
            if (configuration.AppSettings.Settings[key] != null)
            {
                return configuration.AppSettings.Settings[key].Value;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Relay Command Functions
        private void Login(object currentObject)
        {
            chatWorker.RunWorkerAsync();
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
            ircServerEnabled = false;
            ircPortEnabled = false;
            ircUsernameEnabled = false;
            ircOATHEnabled = false;
            saveLoginEnabled = false;
            btnLoginEnabled = false;
            progressBarVisible = true;

            client = new IrcClient(ircServer, Convert.ToInt32(ircPort));
            client.AltNick = ircUsername;
            client.Nick = ircUsername;
            client.ServerPass = ircOATH;

            client.ChannelMessage += Client_ChannelMessage;
            client.NoticeMessage += Client_NoticeMessage;
            client.OnConnect += Client_OnConnect;
            client.PrivateMessage += Client_PrivateMessage;
            client.ServerMessage += Client_ServerMessage;
            client.UserJoined += Client_UserJoined;
            client.UserLeft += Client_UserLeft;
            client.UserNickChange += Client_UserNickChange;
            client.UpdateUsers += Client_UpdateUsers;
            client.NickTaken += Client_NickTaken;

            client.Connect();
        }
        #region Socket Events
        private void Client_NickTaken(object sender, StringEventArgs e)
        {
            
        }

        private void Client_UpdateUsers(object sender, UpdateUsersEventArgs e)
        {
            
        }

        private void Client_UserNickChange(object sender, UserNickChangedEventArgs e)
        {
            
        }

        private void Client_UserLeft(object sender, UserLeftEventArgs e)
        {
            
        }

        private void Client_UserJoined(object sender, UserJoinedEventArgs e)
        {
            
        }

        private void Client_ServerMessage(object sender, StringEventArgs e)
        {
            
        }

        private void Client_PrivateMessage(object sender, PrivateMessageEventArgs e)
        {
            
        }

        private void Client_OnConnect(object sender, EventArgs e)
        {
            client.JoinChannel("#" + ircUsername);
        }

        private void Client_NoticeMessage(object sender, NoticeMessageEventArgs e)
        {
            
        }

        private void Client_ChannelMessage(object sender, ChannelMessageEventArgs e)
        {
            
        }
        #endregion
    }
}
