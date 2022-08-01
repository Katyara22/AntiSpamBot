using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DicordAntiSpamBot
{
    class Program
    {
        private static DiscordSocketClient client;
        static async Task Main(string[] args)
        {
            var config = new DiscordSocketConfig { MessageCacheSize = 200 };

            client = new DiscordSocketClient(config);

            client.Log += Log;

            await client.LoginAsync(TokenType.Bot, EnvVariables.discordBotToekn);
            await client.StartAsync();

            client.MessageReceived += MessageReceived;

            await Task.Delay(-1);
        }

        private static async Task MessageReceived(SocketMessage arg)
        {
            Console.WriteLine($"{arg.Content}");

            if (arg.Author.Id == client.CurrentUser.Id)
                return;

            if (arg.Content == "Ping")
            {
                await arg.Channel.SendMessageAsync("Pong");
                return;
            }

            var myRegex = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            HttpClient httpClient = new HttpClient();

            var postReqvalues = new Dictionary<string, string> {
                {"stricktness","0" },
                {"fast","true" }
            };

            var content = new FormUrlEncodedContent(postReqvalues);

            foreach (Match match in myRegex.Matches(arg.Content))
            {
                string url = match.Value.Replace(":","%3A").Replace("/","%2F");

                HttpResponseMessage response = await httpClient.PostAsync("https://ipqualityscore.com/api/json/url/"+EnvVariables.ipScoreKey+"/" + url, content);

                string responseString = await response.Content.ReadAsStringAsync();

                Root parsedResponse = JsonConvert.DeserializeObject<Root>(responseString);

                if (parsedResponse.@unsafe == true||parsedResponse.suspicious||parsedResponse.risk_score<50)
                {
                    await arg.Channel.SendMessageAsync($"Spam Found !{arg.Author.Username} you moron fuck you did it ?");
                    await arg.DeleteAsync();
                    break;
                }
                Console.WriteLine(responseString);
            }

        }

        private static Task Log(LogMessage arg)
        {

            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }
    }

}
