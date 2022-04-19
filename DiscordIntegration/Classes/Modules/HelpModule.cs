using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class HelpModule : ModuleWrapper
    {
        private List<string> _pages = new List<string>();
        private readonly string FeedbackURL = "https://forms.gle/BQjvTjb7x18zsn3YA";

        public HelpModule(IInfusedRealityServices appServices) : base(appServices)
        {
            BuildPages();
        }

        [Command("Help")]
        [Summary("Outputs Command Help")]
        public Task Help(int PAGE = 1)
        {
            if (PAGE > _pages.Count)
                PAGE = 1;

            StringWriter sw = new StringWriter();
            sw.WriteLine(String.Format("Page {0}/{1}", PAGE, _pages.Count));
            sw.Write(_pages[PAGE - 1]);

            return ReplyAsync(sw.ToString());
        }

        [Command("SubmitFeedback")]
        [Summary("")]
        public Task SubmitFeedback()
        {
            return ReplyAsync(String.Format("Thanks! You can submit feedback at this link or by email at Feedback@InfusedRealityGames.com \n {0}", FeedbackURL));
        }

        #region Helper Methods
        private void BuildPages()
        {
            var commands = _appServices.GetDiscordCommandService().GetAll(w => w.Active).ToList();

            var categorys = commands.Select(s => s.Category).Distinct().ToList();
            var groups = new Dictionary<string, List<DiscordCommand>>();

            foreach (var category in categorys)
            {
                groups.Add(category, commands.Where(w => w.Category == category).OrderBy(o => o.Name).ToList());
            }

            StringWriter sw = new StringWriter();
            StringWriter temp = new StringWriter();
            sw.WriteLine("Thanks for using HoldingHands.AI!");
            sw.WriteLine("Here is a list of all current commands");
            sw.WriteLine("=====================================================");

            foreach (var category in groups)
            {
                temp = WriteCategory(sw, category.Key);

                if (temp.ToString().Length > 1500)
                {
                    _pages.Add(sw.ToString());
                    sw = new StringWriter();
                    sw.WriteLine("=====================================================");
                    sw = WriteCategory(sw, category.Key);
                }

                foreach (var command in category.Value)
                {
                    temp = WriteCommand(sw, command);

                    if (temp.ToString().Length > 1500)
                    {
                        _pages.Add(sw.ToString());
                        sw = new StringWriter();
                        sw.WriteLine("=====================================================");
                        sw = WriteCommand(sw, command);
                    }
                }
            }

            _pages.Add(sw.ToString());
        }

        private StringWriter WriteCategory(StringWriter writer, string category)
        {
            var LineLength = 53;
            var Remainder = LineLength - category.Length;
            var bufferSize = Math.Floor((double)(Remainder / 2));
            var categoryLine = category.PadLeft((int)(bufferSize + category.Length), ' ').PadRight((int)(bufferSize + category.Length), ' ');
            writer.WriteLine(categoryLine);
            writer.WriteLine("=====================================================");
            return writer;
        }
        private StringWriter WriteCommand(StringWriter writer, DiscordCommand command)
        {
            writer.WriteLine("Name: " + command.Name);
            writer.WriteLine("Category: " + command.Category);
            writer.WriteLine("Description: " + command.Description);
            writer.WriteLine("Parameters: " + command.Parameters);
            writer.WriteLine("Example: " + command.Example);
            writer.WriteLine("=====================================================");
            return writer;
        }
        #endregion
    }
}