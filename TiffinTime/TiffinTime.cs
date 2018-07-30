using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinTime
{
    class TiffinTime
    {
        static void Main(string[] args)
        {
            // URLs
            string meatOptionURL = "https://www.tiffintime.co.uk/collections/tiffin-time-lunches/products/meaty-product";
            string vegOptionURL = "https://www.tiffintime.co.uk/collections/tiffin-time-lunches/products/veggie-option";
            string optionXPath = "//*[@id=\"ProductSection-product-template\"]/div/div[2]/div[2]";
            string menuImgUrl = "https://www.tiffintime.co.uk/pages/menu";
            string menuImgXPath = "//*[@id=\"MainContent\"]/div/div/div/div[2]/img";

            // Get this week's menu
            var wh = new WebHelper();
            try
            {
                string meatOption = wh.MenuGetOptionText(meatOptionURL, optionXPath).Trim();
                string vegOption = wh.MenuGetOptionText(vegOptionURL, optionXPath).Trim();
                string imageUrl = wh.GetMenuImageUrl(menuImgUrl, menuImgXPath);
                SendResultsToChannel(meatOption, vegOption, imageUrl);
            }
            catch (Exception)
            {
                SendErrorToSimonOnly("Error trying to get Tiffintime menu options. They've likely changed the page again. Gaddamnit");
                Environment.Exit(-1);
            }
        }

        public static void SendResultsToChannel(string meatOption, string vegOption, string imageUrl)
        {
            Credentials cred = new Credentials();
            string _channel = cred.channel;
            string _token = cred.token;

            // constuct message format
            AttachmentFields afMeat = new AttachmentFields()
            {
                Title = "Meat",
                Value = meatOption
            };
            AttachmentFields afVeg = new AttachmentFields()
            {
                Title = "Veggie",
                Value = vegOption
            };
            Attachment a = new Attachment()
            {
                Pretext = "This week's menu:",
                Fallback = $"TiffinTime menu this week: Meaty - {meatOption}. Veggie - {vegOption}.",
                Fields = { afMeat, afVeg },
                ImageUrl = "https:" + imageUrl,
                Footer = "Vote below!"
            };
            Arguments p = new Arguments()
            {
                Channel = _channel,
                Attachments = { a }
            };

            // send
            var slack = new SlackClientAPI(_token);
            var response = slack.PostMessage("chat.postMessage", p);

            // send reactions (for voting)
            if (response.Ok)
            {
                slack.PostMessage("reactions.add", new Arguments()
                {
                    Name = "cut_of_meat",
                    Channel = _channel,
                    Timestamp = response.TimeStamp
                });

                slack.PostMessage("reactions.add", new Arguments()
                {
                    Name = "green_salad",
                    Channel = _channel,
                    Timestamp = response.TimeStamp
                });
            }
        }

        public static void SendErrorToSimonOnly(string message)
        {
            Credentials cred = new Credentials(dmSimonOnly: true);
            string _channel = cred.channel;
            string _token = cred.token;

            // send
            var slack = new SlackClientAPI(_token);
            var response = slack.PostMessage("chat.postMessage", new Arguments() {
                Channel = _channel,
                Text = message
            });
            
        }
    }
}
