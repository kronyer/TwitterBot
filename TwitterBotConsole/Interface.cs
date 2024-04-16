using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBotConsole
{
    internal class Interface
    {
        static public void RunMenu()
        {
            while (true)
            {
                Services.TimeUntilNext();

                Console.WriteLine("twitter bot is running");
                string tweetWord = Services.GetWord();
                string wordMeaning = Services.GetMeaning(tweetWord);
                string tweet = $"{tweetWord}-- {wordMeaning}";

                Console.WriteLine($"Preview: \n {tweet}");
                Services.Tweet(tweet);
                Console.WriteLine();
            }
            
        }
    }
}
