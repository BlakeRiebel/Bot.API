using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class UserTextCommandModule : ModuleBase<SocketCommandContext>
    {
        private IInfusedRealityServices _appServices;

        public UserTextCommandModule(IInfusedRealityServices appServices)
        {
            _appServices = appServices;
        }


        [Command("Subscribe")]
        [Summary("Selects a random user from the passed in users")]
        public Task Subscribe(string twitchName)
        {
            var username = Context.User.Username;

            var user = _appServices.GetUsersService().GetAll().Where(w => w.UserName.ToLower() == username.ToLower()).FirstOrDefault();

            if (user == null)
                user = _appServices.GetUsersService().Insert(new DiscordBot.Data.Entities.User
                {
                    UserName = username,
                    DiscordId = Context.User.Id.ToString(),
                });


            var twitchChannel = _appServices.GetTwitchChannelsService().GetAll().Where(w => w.Name.ToLower() == twitchName.ToLower()).FirstOrDefault();

            if (twitchChannel == null)
            {
                return ReplyAsync(String.Format("Twitch Channel Not Found!\nPlease check your spelling and then use ?AddTwitchChannel to add the channel."));
            }

            var subscription = _appServices.GetSubscriptionsService().GetAll().Where(w => w.Id == twitchChannel.SubscriptionId).FirstOrDefault();

            user.Subscriptions += String.Format("{0},", subscription.Id);
            subscription.SubscriberIDs += String.Format("{0},", user.UserId);

            _appServices.GetUsersService().Update(user);
            _appServices.GetSubscriptionsService().Update(subscription);

            return ReplyAsync(String.Format("You have subscribed to Live notifications for {0}!", twitchName));
        }
    }
}