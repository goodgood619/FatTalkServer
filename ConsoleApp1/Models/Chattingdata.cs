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
        public string[] chatnickarray;
        public Chattingdata(int chatnumber,string[] chatnickarray)
        {
            this.chatnumber = chatnumber;
            this.chatnickarray = new string[chatnickarray.Length];
            for(int i = 0; i < chatnickarray.Length; i++)
            {
                this.chatnickarray[i] = chatnickarray[i];
            }
        }
    }
}
