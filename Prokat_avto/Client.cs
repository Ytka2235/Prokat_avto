using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prokat_avto
{
    internal class Client
    {
        public int Id;
        public string Number;
        public string Name;

        public Client(int id, string num,string name) 
        {
            Id = id;
            Number = num;
            Name = name;
        }

    }
}
