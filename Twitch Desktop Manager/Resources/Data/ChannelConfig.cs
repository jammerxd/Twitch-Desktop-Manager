using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Twitch_Desktop_Manager.Resources.Data.ChannelConfigClasses;
using System.Collections.ObjectModel;
namespace Twitch_Desktop_Manager.Resources.Data
{
    [ImplementPropertyChanged]
    public class ChannelConfig
    {
        public string ChannelName { get; set; }
        public string ChannelNickName { get; set; }

        public ObservableDictionary<string, string> CustomCommands { get; set; }
        public ObservableDictionary<string,AutomaticMessage> AutomaticMessages { get; set; }
        public BanConfig BanConfig { get; set; }
        
        public ChannelConfig(string chName, string chNickName)
        {
            this.ChannelName = chName;
            this.ChannelNickName = chNickName;
            this.CustomCommands = new ObservableDictionary<string, string>();
            this.AutomaticMessages = new ObservableDictionary<string, AutomaticMessage>();
            this.BanConfig = new BanConfig();
        }
    }
}
