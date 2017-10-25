using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVBot.Core
{
    public static class Channel
    {
        public static async Task<IMessageChannel> GetChannel(ICommandContext context, ulong id)
        {
            var chans = await context.Guild.GetTextChannelsAsync();
            foreach (IMessageChannel chan in chans) { if (chan.Id.Equals(id)) { return chan; } }
            return null;
        }
        public static async Task<IMessageChannel> GetChannel(ICommandContext context, string name)
        {
            ulong chanid = 0;
            string channelname = name.Trim('<', '>', '#');

            if (ulong.TryParse(channelname, out chanid))
            {
                return await GetChannel(context, chanid);
            }

            var chans = await context.Guild.GetTextChannelsAsync();
            foreach (IMessageChannel chan in chans) { if (chan.Name.Equals(channelname.Replace("#", ""), StringComparison.CurrentCultureIgnoreCase)) { return chan; } }
            return null;
        }

        public static async Task ClearChannel(IMessageChannel channel)
        {
            var messages = await channel.GetMessagesAsync(100).Flatten();
            await channel.DeleteMessagesAsync(messages);
        }

    }
}
