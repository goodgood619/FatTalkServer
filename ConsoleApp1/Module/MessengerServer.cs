using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Module
{
    public class MessengerServer : TcpServer
    {
        private const int serverport = 33212;
        private DBhelp dBhelp;
        private JsonHelp jsonHelp;
        public MessengerServer()
        {
            this.dBhelp = new DBhelp();
            this.jsonHelp = new JsonHelp();
        }
        public override List<SocketData> responseMessage(Socket socket, TCPmessage receivemessage)
        {
            List<SocketData> sendclient = new List<SocketData>();
            TCPmessage sendmessage = new TCPmessage();
            switch (receivemessage.command)
            {
                case Command.login:
                    Dictionary<string, string> logininfo = jsonHelp.getlogininfo(receivemessage.message);
                    string id = logininfo[JsonName.ID];
                    string password = logininfo[JsonName.Password];
                    bool idcheck = dBhelp.IsexistID(id);
                    bool passcheck = dBhelp.IsExistPassword(password);
                    bool validlogin = dBhelp.validLogin(id, password);
                    if (!idcheck)
                    {
                        sendmessage.check = 0;
                    }
                    if (!passcheck)
                    {
                        sendmessage.check = 1;
                    }
                    if (!validlogin)
                    {
                        sendmessage.check = 2;

                    }
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Join:

                    break;
                case Command.Idcheck:
                    break;
            }
        }

            return sendclient;
        }
    }
}
