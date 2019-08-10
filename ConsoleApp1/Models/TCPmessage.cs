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
        public int Usernumber { get; set; }
        public int Friendcount { get; set; }
        public int Chatnumber { get; set; }
        public TCPmessage()
        {
            command = Command.Null;
            check = 0;
            message = string.Empty;
            Usernumber = 0;
            Friendcount = 0;
            Chatnumber = 0;
        }

        //1. command 2. check 3. melength, 4.message
        public TCPmessage(byte[] data)
        {
            command = (Command)BitConverter.ToInt32(data, 0);
            check = BitConverter.ToInt32(data, 4);
            Usernumber = BitConverter.ToInt32(data, 8);
            Friendcount = BitConverter.ToInt32(data, 12);
            Chatnumber = BitConverter.ToInt32(data, 16);
            int melength = BitConverter.ToInt32(data, 20);
            if (melength > 0)
            {
                message = Encoding.Unicode.GetString(data, 24, melength);
            }

        }

        public byte[] Tobytedata()
        {
            List<byte> bytedata = new List<byte>();
            bytedata.AddRange(BitConverter.GetBytes((int)command));
            bytedata.AddRange(BitConverter.GetBytes(check));
            bytedata.AddRange(BitConverter.GetBytes((int)Usernumber));
            bytedata.AddRange(BitConverter.GetBytes((int)Friendcount));
            bytedata.AddRange(BitConverter.GetBytes((int)Chatnumber));
            bytedata.AddRange(BitConverter.GetBytes(Encoding.Unicode.GetByteCount(message)));
            bytedata.AddRange(Encoding.Unicode.GetBytes(message));

            return bytedata.ToArray();
        }
    }
    public enum Command
    {
        Null, login, logout, Join, Idcheck, Findid, Makechat,
        Outchat,
        Joinchat,
        Refresh,
        Plusfriend,
        Removefriend,
        Sendchat,
        Nicknamecheck,
        ReceiveJoinchat,
        Blockfriend
    }
}
