using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Twitch_Desktop_Manager
{
    public interface IMainWindowCallbacks
    {
        void ShowMessage(string Title, string Message, Boolean exit);
        void ChangeDimensions(int Width, int Height);
        void ChangeTheme(string theme);
        void ChangeAccent(string accent);
    }
}
