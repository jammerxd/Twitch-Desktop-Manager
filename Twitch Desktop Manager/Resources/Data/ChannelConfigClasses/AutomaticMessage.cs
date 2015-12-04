using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using PropertyChanged;
using TechLifeForum;
namespace Twitch_Desktop_Manager.Resources.Data.ChannelConfigClasses
{
    [ImplementPropertyChanged]
    public class AutomaticMessage
    {
        public string Message { get; set; }
        
        public Timer showMessage { get; set; }
        public IrcClient client { get; set; }

        public AutomaticMessage(int timing,string text,IrcClient currentClient)
        {
            this.interval = timing;
            this.Message = text;
            this.showMessage = new Timer(this.interval);
            this.showMessage.Elapsed += ShowMessage_Elapsed;
            this.client = currentClient;
            
        }

        private void ShowMessage_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(this.client.Connected)
            {
                client.SendMessage("#" + client.Nick, this.Message);
            }
        }
        private int _interval { get; set; }
        public int interval 
        {
            get 
            { 
                return _interval; 
            }
            set 
            {
                this._interval = value;
                Boolean enabled = this.showMessage.Enabled;
                this.showMessage.Enabled = false;
                this.showMessage.Stop();
                this.showMessage = new Timer(value);
                this.showMessage.Elapsed += ShowMessage_Elapsed;
                this.showMessage.Enabled = enabled;
                if(enabled)
                {
                    this.showMessage.Start();
                }
            }
        }

    }
}
