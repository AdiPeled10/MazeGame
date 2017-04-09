using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    interface IClient
    {
        void SetUnblocking();
        void SetBlocking();
        string RecvARequest();  // every request will be separated by a special char, maybaye 0b1111 11111
        void SendResponse(string res);
    }
}
