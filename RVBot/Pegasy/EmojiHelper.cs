using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace RVBot.Pegasy
{
    public static class EmojiHelper
    {
        public const string white_check_mark = "✅";
        public const string no_entry = "⛔";

        public static string ToMessageString(this GuildEmote emoji) => $"<{emoji.ToIdString()}>";
        public static string ToIdString(this GuildEmote emoji) => $":{emoji.Name}:{emoji.Id}";
    }
}
