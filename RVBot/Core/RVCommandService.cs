using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVBot.Core
{
    public static class RVCommandService
    {
        private static CommandService _Service = new CommandService();
        public static CommandService Service
        {
            //get { return (_Service == null ? CreateRVCommandService() : _Service); }
            get { return _Service; }
        }


    }
}
