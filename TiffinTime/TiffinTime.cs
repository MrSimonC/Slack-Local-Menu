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
            string meatOptionURL = "https://www.tiffintime.co.uk/collections/tiffin-time-lunches/products/meaty-option";
            string vegOptionURL = "https://www.tiffintime.co.uk/collections/tiffin-time-lunches/products/veggie-option";
            string homepage = "https://www.tiffintime.co.uk/";

            // Get this week's menu
            var wh = new WebHelper();
            string meatOption = wh.MenuGetOptionText(meatOptionURL).TrimEnd('\n');
            string vegOption = wh.MenuGetOptionText(vegOptionURL).TrimEnd('\n');
            string imageUrl = wh.GetMenuImageUrl(homepage);

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
    }
}
