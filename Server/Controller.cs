using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Controller : IController
    {
        private IView view;
        private IModel model;
        private Dictionary<string, ICommand> commands;

        public Controller()
        {
        }
    }
}
