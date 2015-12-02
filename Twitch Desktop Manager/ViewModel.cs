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
using System.Xml;
using MahApps.Metro;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Xaml;

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
        private XmlWriter channelConfigWriter { get; set; }
        private string defaultConfigXMLPath { get; set; }

        #region Theme Settings
        public string theme {get;set;}
        public string accent { get; set; }


        #endregion
        #endregion
        //--------------END OF VARIABLES--------------\\


        #region imported code for theme changing
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        
        #endregion

        public ViewModel(IMainWindowCallbacks mainCallbacks)
        {
            if (mainCallbacks == null)
            {
                throw new ArgumentException("mainwindow callbacks can't be null");
            }
            mainWindowCallbacks = mainCallbacks;
            version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();

            configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            
            defaultConfigXMLPath = Directory.GetCurrentDirectory() + "\\default.xml";

            mainWindowCallbacks.ChangeDimensions(300, 350);

            #region imported code for theme changing
            
            this.AccentColors = ThemeManager.Accents.Select(a => new AccentColorMenuData() { displayName=a.Name, ChangeAccentCommand=this.ChangeAccentCommand,ChangeThemeCommand=this.ChangeThemeCommand, Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush }).ToList();

            
            this.AppThemes = ThemeManager.AppThemes.Select(a => new AppThemeMenuData() {displayName=a.Name.Substring(4), ChangeAccentCommand = this.ChangeAccentCommand, ChangeThemeCommand = this.ChangeThemeCommand, Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush }).ToList();




            #endregion

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

            theme = GetSetting("theme");
            accent = GetSetting("accent");
            if(theme == "")
            {
                theme = "Light";
                AddorUpdateSetting("theme", theme);
            }
            if (accent == "") 
            {
                accent = "Purple";
                AddorUpdateSetting("accent", accent);
            }
            ChangeAccent(accent);
            ChangeTheme("Base" + theme);



            ircServer = GetSetting("url");
            ircPort = GetSetting("port");
            ircUsername = GetSetting("username");
            if (ircUsername != "")
            {
                saveLogin = true;
                ircOATH = Encryption.Decrypt(GetSetting("oath"), Encryption.GetHashString(ircUsername));
            }
            if(!File.Exists(defaultConfigXMLPath))
            {
                channelConfigWriter = XmlWriter.Create(defaultConfigXMLPath);
                channelConfigWriter.WriteStartDocument();
                channelConfigWriter.WriteStartElement("Channels");

                channelConfigWriter.WriteEndElement();
                channelConfigWriter.WriteEndDocument();
            }
        }


        #region Settings Functions
        public void AddorUpdateSetting(string key, string value = "")
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

        public string GetSetting(string key)
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
        public void ChangeAccent(object currentObject)
        {
            mainWindowCallbacks.ChangeAccent(currentObject.ToString());
            AddorUpdateSetting("accent", currentObject.ToString());
        }
        public void ChangeTheme(object currentObject)
        {
            mainWindowCallbacks.ChangeTheme(currentObject.ToString());
            AddorUpdateSetting("theme", currentObject.ToString().Substring(4));

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
        private RelayCommand _ChangeAccentCommand { get; set; }
        public RelayCommand ChangeAccentCommand
        {
            get
            {
                if (_ChangeAccentCommand == null)
                {
                    _ChangeAccentCommand = new RelayCommand(ChangeAccent);
                }
                return _ChangeAccentCommand;
            }
        }
        private RelayCommand _ChangeThemeCommand { get; set; }
        public RelayCommand ChangeThemeCommand 
        {
            get
            {
                if(_ChangeThemeCommand == null)
                {
                    _ChangeThemeCommand = new RelayCommand(ChangeTheme);
                }
                return _ChangeThemeCommand;
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

            if (ircServer == "" || ircPort == "" || ircUsername == "")
            {
                ircServerEnabled = true;
                ircPortEnabled = true;
                ircUsernameEnabled = true;
                ircOATHEnabled = true;
                saveLoginEnabled = true;
                btnLoginEnabled = true;
                progressBarVisible = false;
                mainWindowCallbacks.ShowMessage("Login Error", "Please make sure that all fields are filled in accordingly.", false);

            }
            else
            {

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
            if(saveLogin)
            {
                AddorUpdateSetting("url", ircServer);
                AddorUpdateSetting("port", ircPort);
                AddorUpdateSetting("username", ircUsername);
                AddorUpdateSetting("oath", Encryption.Encrypt(ircOATH, Encryption.GetHashString(ircUsername)));

            }
            else
            {
                AddorUpdateSetting("url");
                AddorUpdateSetting("port");
                AddorUpdateSetting("username");
                AddorUpdateSetting("oath");
            }


           
        }

        private void Client_NoticeMessage(object sender, NoticeMessageEventArgs e)
        {
            
        }

        private void Client_ChannelMessage(object sender, ChannelMessageEventArgs e)
        {
            
        }
        #endregion






        #region imported for theme changing

        public class AccentColorMenuData
        {
            public string Name { get; set; }
            public string displayName { get; set; }
            public Brush BorderColorBrush { get; set; }
            public Brush ColorBrush { get; set; }

            public RelayCommand ChangeThemeCommand {get;set;}
            public RelayCommand ChangeAccentCommand { get; set; }



            protected virtual void DoChangeTheme(object sender)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var accent = ThemeManager.GetAccent(this.Name);
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
            }



        }

        public class AppThemeMenuData : AccentColorMenuData
        {
            protected override void DoChangeTheme(object sender)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var appTheme = ThemeManager.GetAppTheme(this.Name);
                ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
            }
        }

        public class SimpleCommand : ICommand
        {
            public Predicate<object> CanExecuteDelegate { get; set; }
            public Action<object> ExecuteDelegate { get; set; }

            public bool CanExecute(object parameter)
            {
                if (CanExecuteDelegate != null)
                    return CanExecuteDelegate(parameter);
                return true; // if there is no can execute default to true
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                if (ExecuteDelegate != null)
                    ExecuteDelegate(parameter);
            }
        }
        #endregion


    }
}
