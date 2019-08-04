using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Chattingdata
    {
        public int chatnumber;
        public List<string> chatnickarray;
        public Chattingdata(int chatnumber,List<string> chatnickarray)
        {
            this.chatnumber = chatnumber;
            this.chatnickarray = new List<string>();
            for(int i = 0; i < chatnickarray.Count; i++)
            {
                this.chatnickarray.Add(chatnickarray[i]);
            }
        }
    }
}
