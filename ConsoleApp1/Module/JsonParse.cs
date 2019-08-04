using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Json;
namespace ConsoleApp1.Module
{
    public class JsonParse
    {
        private JsonTextParser jsonTextParser;
        private JsonObjectCollection jsonObjectCollection;

        public JsonParse(string data)
        {
            this.jsonTextParser = new JsonTextParser();
            this.jsonObjectCollection = (JsonObjectCollection)jsonTextParser.Parse(data);
        }

        public string GetstringValue(string name)
            {
            string s = string.Empty;
            try
            {
                s = Convert.ToString(jsonObjectCollection[name].GetValue());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return s;
        }

        public string[] GetstringArrayvalue(string name)
        {
            string[] arrayvalue = null;
            try
            {
                JsonArrayCollection jsonArrayCollection = (JsonArrayCollection)jsonObjectCollection[name];
                int cnt = jsonArrayCollection.Count();
                arrayvalue = new string[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    arrayvalue[i] = ((JsonStringValue)jsonArrayCollection[i]).Value;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return arrayvalue;
        }
        public List<string> GetstringListvalue(string name)
        {
            List<string> list = new List<string>();
            try
            {
                JsonArrayCollection jsonArrayCollection = (JsonArrayCollection)jsonObjectCollection[name];
                int cnt = jsonArrayCollection.Count();
                for(int i = 0; i < cnt; i++)
                {
                    list.Add(((JsonStringValue)jsonArrayCollection[i]).Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return list;
        }
    }
}
