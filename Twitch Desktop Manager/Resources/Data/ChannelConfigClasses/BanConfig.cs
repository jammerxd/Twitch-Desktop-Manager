using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PropertyChanged;
namespace Twitch_Desktop_Manager.Resources.Data.ChannelConfigClasses
{
    [ImplementPropertyChanged]
    public class BanConfig
    {
        public int kickTime { get; set; }
        public int strikesBeforeBan { get; set; }
        public ObservableCollection<string> bannedWords { get; set; }
        public ObservableCollection<string> bannedSongIDs { get; set; }
        public ObservableCollection<string> bannedSongRequestUsers { get; set; }
        public ObservableCollection<string> bannedSongWords { get; set; }
        public ObservableCollection<string> bannedUsers { get; set; }
        public ObservableCollection<string> bannedCommandUsers { get; set; }
        public Boolean allowLinks { get; set; }
    }
}
