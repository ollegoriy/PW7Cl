using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW7Cl
{
    public class Chat
    {
        public string UserName { get; set; }
        public string Time { get; set; }
        public string Text { get; set; }
        public Chat(string userName, string time, string text)
        {
            UserName = userName; Time = time;
            Text = text;
        }
    }
}
