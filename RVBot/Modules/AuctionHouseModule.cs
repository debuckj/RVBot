using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RVBot.Modules
{
    public class AuctionHouseModule : ModuleBase
    {
        public AuctionHouseModule()
        {
            // load data
        }

        [Command("auction create"), Summary("")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task x(string channelname = null)
        { }
        
        [Command("auction start"), Summary("")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task x2()
        { }

        [Command("auction stop"), Summary("")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task x3()
        { }

        [Command("reg"), Summary("")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task x4()
        { }

        [Command("bid"), Summary("")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task x5(int value)
        { }




    }
}
