using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class TCPmessage
    {
        public Command command { get; set; }
        public int check { get; set; }
        public string message { get; set; }

        public TCPmessage()
        {
            command = Command.Null;
            check = 0;
            message = string.Empty;
        }

        //1. command 2. check 3. melength, 4.message
        public TCPmessage(byte[] data)
        {
            command = (Command)BitConverter.ToInt32(data, 0);
            check = BitConverter.ToInt32(data, 4);
            int melength = BitConverter.ToInt32(data, 8);
            if (melength > 0)
            {
                message = Encoding.Unicode.GetString(data, 12, melength);
            }

        }

        public byte[] Tobytedata()
        {
            List<byte> bytedata = new List<byte>();
            bytedata.AddRange(BitConverter.GetBytes((int)command));
            bytedata.AddRange(BitConverter.GetBytes(check));
            bytedata.AddRange(BitConverter.GetBytes(Encoding.Unicode.GetByteCount(message)));
            bytedata.AddRange(Encoding.Unicode.GetBytes(message));


            return bytedata.ToArray();
        }
    }
    public enum Command
    {
        Null, login, logout,Join,Idcheck,Findid
    }
}
