using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace TwitchIntegration.Classes
{
    public class TwitchPubSubController
    {
        private static List<TwitchPubSub> clients;
        private IInfusedRealityServices _appServices;

        public static event EventHandler<TwitchChannel> StreamStarted;
        public static event EventHandler<TwitchChannel> StreamEnded;

        public static List<TwitchChannel> channels { get; set; }

        public TwitchPubSubController(IInfusedRealityServices appServices)
        {
            _appServices = appServices;
            AddChannelList();
            clients = new List<TwitchPubSub>();
            channels.ForEach(channel => CreateClient(channel));
        }

        public async Task Run()
        {
            clients.ForEach(client => client.Connect());
            await Task.Delay(Timeout.Infinite);
        }

        private void AddChannelList()
        {
            channels = _appServices.GetTwitchChannelsService().GetAll().ToList();
        }

        private void CreateClient(TwitchChannel channel)
        {
            var client = new TwitchPubSub();
            client.OnPubSubServiceConnected += onPubSubServiceConnected;
            client.OnListenResponse += onListenResponse;
            client.OnStreamUp += onStreamUp;
            client.OnStreamDown += onStreamDown;

            client.ListenToVideoPlayback(channelTwitchId: channel.TwitchId.ToString());

            clients.Add(client);
        }

        private static void onPubSubServiceConnected(object sender, EventArgs e)
        {
            // SendTopics accepts an oauth optionally, which is necessary for some topics
            var client = sender as TwitchPubSub;
            client.SendTopics();

            Log.Write(Serilog.Events.LogEventLevel.Information, "Connected");
        }
        private static void onListenResponse(object sender, OnListenResponseArgs e)
        {
            Log.Write(Serilog.Events.LogEventLevel.Information, "Response: {e}", e.Successful);
            if (!e.Successful)
                throw new Exception($"Failed to listen! Response: {e.Response}");
        }
        private static void onStreamUp(object sender, OnStreamUpArgs e)
        {
            Log.Write(Serilog.Events.LogEventLevel.Information, "Stream just went up!");

            var channel = channels.Where(channel => channel.TwitchId.ToString() == e.ChannelId).FirstOrDefault();

            StreamStarted?.Invoke(sender, channel);
        }
        private static void onStreamDown(object sender, OnStreamDownArgs e)
        {
            Log.Write(Serilog.Events.LogEventLevel.Information, "Stream just went down!");

            var channel = channels.Where(channel => channel.TwitchId.ToString() == e.ChannelId).FirstOrDefault();

            StreamEnded?.Invoke(sender, channel);
        }
    }
}
