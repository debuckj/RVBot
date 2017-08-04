﻿using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RVBot.Core;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    //private CommandService commands;
    private DiscordSocketClient client;
    private IServiceProvider map;

    // Convert our sync main to an async main.
    public static void Main(string[] args) =>
        new Program().Start().GetAwaiter().GetResult();

    public async Task Start()
    {
        client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            ConnectionTimeout = (int)20000,
#if NET462
            WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance,
#endif
            MessageCacheSize = 20
        });

        client.Log += (message) =>
        {
            Console.WriteLine($"{message.ToString()}");
            return Task.CompletedTask;
        };

        string token = "Mjk0NTg2MTQzODkzMDk0NDAw.DGZstw.XadYSmwxohsLIrCqB5a-RsS00DE";

        map = new ServiceCollection().BuildServiceProvider();

        await InstallCommands();

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync().ConfigureAwait(false);

        await Task.Delay(-1);
    }

    public async Task InstallCommands()
    {
        // Hook the MessageReceived Event into our Command Handler
        client.MessageReceived += HandleCommand;
        // Discover all of the commands in this assembly and load them.
        //await commands.AddModulesAsync(Assembly.GetEntryAssembly());

        //client.ChannelCreated += HandleEventChannelCreated;
        //client.ChannelDestroyed += HandleEventChannelDestroyed;
        //client.ChannelUpdated += HandleEventChannelUpdated;

        //client.Connected += HandleEventConnected;

        //client.MessageDeleted += HandleEventMessageDeleted;
        //client.MessageReceived += HandleEventMessageReceived;
        //client.MessageUpdated += HandleEventMessageUpdated;

        //client.RoleCreated += HandleEventRoleCreated;
        //client.RoleDeleted += HandleEventRoleDeleted;
        //client.RoleUpdated += HandleEventRoleUpdated;

        //client.UserBanned += HandleEventUserBanned;
        //client.UserIsTyping
        //client.UserJoined += HandleEventUserJoined;
        //client.UserLeft += HandleEventUserLeft;
        //client.UserUnbanned += HandleEventUserUnbanned;
        //client.UserUpdated += HandleEventUserUpdated;
        //client.UserVoiceStateUpdated

        await RVCommandService.Service.AddModulesAsync(Assembly.GetEntryAssembly());
    }

    public async Task HandleCommand(SocketMessage messageParam)
    {
        // Don't process the command if it was a System Message
        var message = messageParam as SocketUserMessage;

        if (message == null) return;
        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;
        // Determine if the message is a command, based on if it starts with '!' or a mention prefix
        if (!(message.HasStringPrefix("rv!", ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
        // Create a Command Context

        var context = new CommandContext(client, message);
        // Execute the command. (result does not indicate a return value,
        // rather an object stating if the command executed succesfully)
        var result = await RVCommandService.Service.ExecuteAsync(context, argPos, map);
        if (!result.IsSuccess)
            await Log.LogMessage(context, result.ErrorReason);
        //await context.Channel.SendMessageAsync(result.ErrorReason);

    }
}

