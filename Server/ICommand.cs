using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    interface ICommand
    {
        void Execute(string[] args, IClient client = null);
    }
}
