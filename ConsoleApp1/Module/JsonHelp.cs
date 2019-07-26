using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Json;
using ConsoleApp1.Models;
namespace ConsoleApp1.Module
{
    public class JsonHelp
    {
        private DBhelp dBhelp;
        //private JsonHelp jsonHelp;
        public JsonHelp(){
            //this.jsonHelp = new JsonHelp();
            this.dBhelp= new DBhelp();
        }

        public Dictionary<string, string> getlogininfo(string data)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.ID, jsonParse.GetstringValue(JsonName.ID));
            ret.Add(JsonName.Password, jsonParse.GetstringValue(JsonName.Password));

            return ret;
        }

        public Dictionary<string, string> getphonenick(string data)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.Nickname, jsonParse.GetstringValue(JsonName.Nickname));
            ret.Add(JsonName.Phone, jsonParse.GetstringValue(JsonName.Phone));
            return ret;
        }
        public Dictionary<string,string> getidinfo(string data){
            Dictionary<string,string> ret= new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.ID,jsonParse.GetstringValue(JsonName.ID));
            return ret;
        }
        public string logininfo(string id,string password)
        {
            JsonObjectCollection ret = new JsonObjectCollection();
            ret.Add(new JsonStringValue(JsonName.ID, id));
            ret.Add(new JsonStringValue(JsonName.Password, password));
            return ret.ToString();
        }
    }
    public class JsonName
    {
        public const string ID = "ID";
        public const string Password = "Password";
        public const string Nickname = "Nickname";
        public const string Phone = "Phone";
    }
}
