﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Module
{
    public class JsonHelp
    {

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
    }
    public class JsonName
    {
        public const string ID = "ID";
        public const string Password = "Password";
        public const string Nickname = "Nickname";
        public const string Phone = "Phone";
    }
}