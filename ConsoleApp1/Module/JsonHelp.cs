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
        public JsonHelp()
        {
            //this.jsonHelp = new JsonHelp();
            this.dBhelp = new DBhelp();
        }


        
        public Dictionary<string, string> getidinfo(string data)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.ID, jsonParse.GetstringValue(JsonName.ID));
            return ret;
        }
        public Dictionary<string,string> getpasswordinfo(string data)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.Password, jsonParse.GetstringValue(JsonName.Password));
            return ret;
        }
        public Dictionary<string, string> getFnickinfo(string data)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.FID, jsonParse.GetstringValue(JsonName.FID));
            return ret;
        }
        public Dictionary<string, string> getnickinfo(string data)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.Nickname, jsonParse.GetstringValue(JsonName.Nickname));
            return ret;
        }
        public Dictionary<string, string> getphoneinfo(string data)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            JsonParse jsonParse = new JsonParse(data);
            ret.Add(JsonName.Phone, jsonParse.GetstringValue(JsonName.Phone));
            return ret;
        }
        public string logininfo(string id, string password)
        {
            JsonObjectCollection ret = new JsonObjectCollection();
            ret.Add(new JsonStringValue(JsonName.ID, id));
            ret.Add(new JsonStringValue(JsonName.Password, password));
            return ret.ToString();
        }
        public string nickinfo(string nickname)
        {
            JsonObjectCollection ret = new JsonObjectCollection();
            ret.Add(new JsonStringValue(JsonName.Nickname, nickname));
            return ret.ToString();
        }
        public string Refreshnickarrayinfo(string[] fnickarray)
        {
            JsonObjectCollection ret = new JsonObjectCollection();
            JsonArrayCollection jsonArray = new JsonArrayCollection("refreshnickarray");
            for(int i = 0; i <fnickarray.Length; i++)
            {
                jsonArray.Add(new JsonStringValue(null, fnickarray[i]));
            }
            ret.Add(jsonArray);
            return ret.ToString();
        }
        public string[] getdeletenickarrayinfo(string data)
        {
            JsonParse jsonParse = new JsonParse(data);
            string[] s = jsonParse.GetstringArrayvalue("deletenickarray");
            return s;
        }
    }
    public class JsonName
    {
        public const string ID = "ID";
        public const string FID = "Fid";
        public const string Password = "Password";
        public const string Nickname = "Nickname";
        public const string Phone = "Phone";
        public const string Usernumber = "Usernumber";
    }
}
