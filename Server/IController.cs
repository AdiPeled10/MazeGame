﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IController
    {
        void ExecuteCommand(string commandLine, IClient client);
    }
}