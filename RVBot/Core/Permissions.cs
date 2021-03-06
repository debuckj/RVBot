﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVBot.Core
{
    public class Permissions
    {
        private static string[] serverStaff = new string[] { "Admin", "Warlord", "UnterAdmin" };


        public static async Task<bool> IsRole(ICommandContext context, string role)
        {
            IGuildUser _user = await User.GetUser(context, context.User);
            return User.HasRole(_user, Role.GetRole(context, role));
        }
        public static async Task<bool> IsRole(ICommandContext context, string role, IGuildUser user)
        {
            IGuildUser _user = await User.GetUser(context, user);
            return User.HasRole(_user, Role.GetRole(context, role));
        }
        public static async Task<bool> IsRole(ICommandContext context, string role, string username)
        {
            IGuildUser _user = await User.GetUser(context, username);
            return User.HasRole(_user, Role.GetRole(context, role));
        }
        public static async Task<bool> IsRole(ICommandContext context, string[] roles)
        {
            IGuildUser _user = await User.GetUser(context, context.User);
            foreach (string role in roles) { if (User.HasRole(_user, Role.GetRole(context, role))) { return true; } }
            return false;
        }
        public static async Task<bool> IsRole(ICommandContext context, string[] roles, IGuildUser user)
        {
            IGuildUser _user = await User.GetUser(context, user);
            foreach (string role in roles) { if (User.HasRole(_user, Role.GetRole(context, role))) { return true; } }
            return false;
        }
        public static async Task<bool> IsRole(ICommandContext context, string[] roles, string username)
        {
            IGuildUser _user = await User.GetUser(context, username);
            foreach (string role in roles) { if (User.HasRole(_user, Role.GetRole(context, role))) { return true; } }
            return false;
        }


        public static async Task<bool> IsOfficer(ICommandContext context) { return await IsRole(context, "Officer"); }
        public static async Task<bool> IsOfficer(ICommandContext context, IGuildUser user) { return await IsRole(context, "Officer", user); }
        public static async Task<bool> IsOfficer(ICommandContext context, string username) { return await IsRole(context,  "Officer", username); }

        public static async Task<bool> IsWarlord(ICommandContext context) { return await IsRole(context, "Warlord"); }
        public static async Task<bool> IsWarlord(ICommandContext context, IGuildUser user) { return await IsRole(context, "Warlord", user); }
        public static async Task<bool> IsWarlord(ICommandContext context, string username) { return await IsRole(context, "Warlord", username); }

        public static async Task<bool> IsAdmin(ICommandContext context) { return await IsRole(context, "Admin"); }
        public static async Task<bool> IsAdmin(ICommandContext context, IGuildUser user) { return await IsRole(context, "Admin", user); }
        public static async Task<bool> IsAdmin(ICommandContext context, string username) { return await IsRole(context, "Admin", username); }

        public static async Task<bool> IsUnterAdmin(ICommandContext context) { return await IsRole(context, "UnterAdmin"); }
        public static async Task<bool> IsUnterAdmin(ICommandContext context, IGuildUser user) { return await IsRole(context, "UnterAdmin", user); }
        public static async Task<bool> IsUnterAdmin(ICommandContext context, string username) { return await IsRole(context, "UnterAdmin", username); }

        public static async Task<bool> IsServerStaff(ICommandContext context) { return await IsRole(context, serverStaff); }
        public static async Task<bool> IsServerStaff(ICommandContext context, IGuildUser user) { return await IsRole(context, serverStaff, user); }
        public static async Task<bool> IsServerStaff(ICommandContext context, string username) { return await IsRole(context, serverStaff, username); }
    }
}
