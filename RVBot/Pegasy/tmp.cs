using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace RVBot
{
    public static class IChannelExt
    {
        public static string ToUsernameDiscriminatorAndNickname(this IUser user)
        {
            var s = $"{user.Username}#{user.Discriminator}";
            var guildUser = user as IGuildUser;
            if (!string.IsNullOrWhiteSpace(guildUser?.Nickname))
            {
                s += $"({guildUser.Nickname})";
            }
            return s;
        }

        private static int limit = 2000 - 8;
        public static async Task SendMessageSplitCodeblockAsync(this IMessageChannel channel, string text, string codeName = "", bool isTTS = false)
        {
            if (text.Length <= limit)
            {
                await channel.SendMessageAsync($"```{codeName}\n{text}```", isTTS);
                return;
            }

            var lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            var msg = "";
            foreach (var line in lines)
            {
                if (msg.Length + line.Length > limit)
                {
                    await channel.SendMessageAsync($"```{codeName}\n{msg}```", isTTS);
                    msg = "";
                }
                if (line.Length > limit)
                {
                    var split = line.SplitInParts(limit);
                    foreach (var s in split)
                    {
                        await channel.SendMessageAsync($"```{codeName}\n{s}```", isTTS);
                    }
                    msg = "";
                }
                else
                {
                    msg += $"\n{line}";
                }
            }
            await channel.SendMessageAsync($"```{codeName}\n{msg}```", isTTS);

        }

        public static async Task SendMessageSplitAsync(this IMessageChannel channel, string text, bool isTTS = false)
        {
            if (text.Length <= limit)
            {
                await channel.SendMessageAsync($"{text}", isTTS);
                return;
            }

            var lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            var msg = "";
            foreach (var line in lines)
            {
                if (msg.Length + line.Length > limit)
                {
                    await channel.SendMessageAsync($"{msg}", isTTS);
                    msg = "";
                }
                if (line.Length > limit)
                {
                    var split = line.SplitInParts(limit);
                    foreach (var s in split)
                    {
                        await channel.SendMessageAsync($"{s}", isTTS);
                    }
                    msg = "";
                }
                else
                {
                    msg += $"\n{line}";
                }
            }
            await channel.SendMessageAsync($"{msg}", isTTS);

        }


        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }


    }
}
