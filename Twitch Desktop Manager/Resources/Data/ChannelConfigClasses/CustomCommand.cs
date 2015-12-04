using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
namespace Twitch_Desktop_Manager.Resources.Data.ChannelConfigClasses
{
    [ImplementPropertyChanged]
    public class CustomCommand
    {
        public string Command { get; set; }
        public string Response { get; set; }
    }
}
