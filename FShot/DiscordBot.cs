using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FShot
{
    class DiscordBot
    {

        private static DiscordClient discord;
        private static DiscordChannel channel;
        private static DiscordUser user;
        private static List<String> lines;
        private static Random rand;



        public static async Task initialize(string token) {
            discord = new DiscordClient(new DiscordConfiguration {
                Token = token,
                TokenType = TokenType.Bot
            });
            StreamReader sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.cfg"));
            lines = new List<string>();
            string line;
            while ((line = sr.ReadLine()) != null)
                if (!line.StartsWith("//")) lines.Add(line);
            sr.Close();
            channel = discord.GetChannelAsync(Convert.ToUInt64(lines.ElementAt(0).Split(' ')[1])).Result;
            user = discord.GetUserAsync(Convert.ToUInt64(lines.ElementAt(1).Split(' ')[1])).Result;
            rand = new Random();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }



        public static void sendScreenshot() {
            Bitmap screenShot = Screen.caputreScreen();
            MemoryStream stream = new MemoryStream();
            screenShot.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Position = 0;
            string message = lines.ElementAt(rand.Next(3, lines.Count - 1));
            message = message.Replace("@User", user.Mention);
            if (Screen.isActiveWindowFullscreen()) message = message.Replace("@Game", "Game: " + Screen.getActiveWindowTitle());
            else message = message.Replace("@Game", "");
            channel.SendFileAsync(stream, "screenshot.png", message);
        }

    }

}

