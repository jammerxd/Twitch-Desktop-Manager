﻿using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace Twitch_Desktop_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ViewModel vm;
        private MainWindowCallbacks mainCallbacks = null;
        public MainWindow()
        {
            InitializeComponent();
            mainCallbacks = new MainWindowCallbacks(this);
            vm = new ViewModel(mainCallbacks);
            this.DataContext = vm;
        }

        #region MainWindowCallbacks Class
        public class MainWindowCallbacks : IMainWindowCallbacks
        {
            #region MainWindowCallbacks
            private MetroWindow _parent = null;
            public MainWindowCallbacks(MetroWindow parent)
            {
                _parent = parent;
            }
            #endregion

            #region ChangeDimensions
            public void ChangeDimensions(int Width, int Height)
            {
                _parent.Dispatcher.Invoke(() =>
                {
                    _parent.Width = Width;
                    _parent.Height = Height;
                });
            }
            #endregion

            #region ShowMessage
            public void ShowMessage(string Title, string Message, Boolean exit)
            {
                //invoke on MainWindow's Dispatcher to show the message
                _parent.Dispatcher.Invoke(async () =>
                {
                    await this._parent.ShowMessageAsync(Title, Message, MessageDialogStyle.Affirmative, new MetroDialogSettings { ColorScheme = MetroDialogColorScheme.Accented });
                    if (exit)
                    {
                        Environment.Exit(0);
                    }
                });
            }
            #endregion

            #region ChangeTheme
            public void ChangeTheme(string theme)
            {
                App.Current.Dispatcher.Invoke(() => {
                    var tempTheme = ThemeManager.DetectAppStyle(Application.Current);
                    var appTheme = ThemeManager.GetAppTheme(theme);
                    ThemeManager.ChangeAppStyle(Application.Current, tempTheme.Item2, appTheme);
                    
                });
                
            }
            #endregion
            #region ChangeAccent
            public void ChangeAccent(string accent)
            {
                App.Current.Dispatcher.Invoke(() => {
                    var tempTheme = ThemeManager.DetectAppStyle(Application.Current);
                    var tempAccent = ThemeManager.GetAccent(accent);
                    ThemeManager.ChangeAppStyle(Application.Current, tempAccent, tempTheme.Item1);
                });
            }
            #endregion
        }
        #endregion
    }
}
