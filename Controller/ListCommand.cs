using System.Collections.Generic;
using Models;
using ClientForServer;
using Newtonsoft.Json;

namespace Controllers
{
    /// <summary>
    /// Implementation of the List command which implements the ICommand interface.
    /// </summary>
    public class ListCommand : ICommand
    {
        /// <summary>
        /// The model which we will use.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Constructor of the List command which gets the model that
        /// we are going to use in execution.
        /// </summary>
        /// <param name="model">
        /// The model which we are going to use.
        /// </param>
        public ListCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Execute the list command based on the given arguments
        /// and the client that sent it.
        /// </summary>
        /// <param name="args">
        /// The arguments of the command.
        /// </param>
        /// <param name="client">
        /// The client that sent this command.
        /// </param>
        public void Execute(string[] args,IClient client)
        {
            List<string> games = model.GetJoinableGamesList();
            client.SendResponse(JsonConvert.SerializeObject(games, Formatting.Indented));
        }
    }
}
