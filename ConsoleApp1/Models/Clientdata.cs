using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace ConsoleApp1.Models
{
    public class Clientdata
    {
        public Socket socket;
        public string id;
        public Clientdata(Socket socket,string id)
        {
            this.socket = socket;
            this.id = id;
        }
    }
}
