﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Server
{
    class CloseCommand : ICommand
    {
        private IModel model;

        public CloseCommand(IModel model)
        {
            this.model = model;
        }

        public string Execute(string[] args, IClient client = null)
        {
            model.Close(args[0]);
            JObject playObj = new JObject();
            return playObj.ToString();
            // return "{ }";
        }
    }
}