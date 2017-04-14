using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    public class JoinCommand : ICommand
    {
        private IModel model;

        public JoinCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client)
        {

            // TODO - It's very similar to the case of StartGameCommand maybe create abstract class for both.
            string name = args[0];
            try
            {
                if (model.Join(name, client))
                {
                    ISearchGame game = model.GetGameByName(name);
                    client.SendResponse((!ReferenceEquals(game, null)) ? 
                        model.GetGameByName(name).ToJSON() : "No such game");
                }
            }
            catch
            { 
                // nothing to do
            }
            client.SendResponse("Falied to join. No such game or You already a part of an existing game");
        }
    }
}
