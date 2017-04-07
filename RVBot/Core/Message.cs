using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;


namespace RVBot.Core
{
    public static class Message
    {
        private static string EmojiWhitespace = "<:whitespace:298909396644265984>";
        private static string EmojiExclamation = ":exclamation:";
        private static string DefaultHeaderRed = String.Format("{0}{1}{2}", EmojiWhitespace, EmojiExclamation, EmojiWhitespace);
        private static string DefaultHeaderGrey = ":whitespace::grey_exclamation::whitespace:";


        public static async Task<string> FormatAnnouncementMessage(CommandContext context, string title, string message)
        {
            string formattedTitle = String.Format("{0}**{1}**{2}", DefaultHeaderRed, title, DefaultHeaderRed);
            return String.Format("{0}{1}{2}", formattedTitle, Environment.NewLine, message);

        }

    }
}
