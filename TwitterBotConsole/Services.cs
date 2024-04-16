using HtmlAgilityPack;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBotConsole
{
    internal class Services
    {
        internal static string GetMeaning(string tweetWord)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load($"https://www.dicio.com.br/{tweetWord}");

            DateTime endTime = DateTime.Now.AddSeconds(10);
            while (DateTime.Now < endTime)
            {
                HtmlNode divNode = document.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div[1]/div[1]/div[1]/p/span[2]");
                if (divNode != null)
                {
                    return divNode.InnerText;
                }

                Thread.Sleep(1000);

                document = web.Load($"https://www.dicio.com.br/{tweetWord}");
            }

            throw new TimeoutException("O elemento não foi encontrado após o tempo limite.");
            
        }

        internal static string GetWord()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.aleatorios.org/");

            DateTime endTime = DateTime.Now.AddSeconds(10);
            while (DateTime.Now < endTime)
            {
                HtmlNode divNode = document.DocumentNode.SelectSingleNode("/html/body/div[1]/section/section[1]/div/h2");
                if (divNode != null)
                {
                    return divNode.InnerText;
                }
                
                Thread.Sleep(1000);
                
                document = web.Load("https://www.aleatorios.org/");
            }

            throw new TimeoutException("O elemento não foi encontrado após o tempo limite.");
        
    }

        internal static void Tweet(string tweetText)
        {
            var client = new RestClient("https://localhost:7179");
            var request = new RestRequest("/api/tweets", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter($"application/json", $"{{ \"text\": \"{tweetText}\" }}", ParameterType.RequestBody);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("ERROR" + response.ErrorMessage);
            }
        }

        public static void TimeUntilNext()
        {
            //here you declare what time of the day you want to receive it
            TimeSpan hour = new TimeSpan(17, 0, 0);
            DateTime now = DateTime.Now;
            DateTime nextSend = now.Date.Add(hour);
            if (now > nextSend)
            {
                nextSend = nextSend.AddDays(1);
            }
            TimeSpan timeUntilNextSend = nextSend - now;

            Console.WriteLine($"Próximo envio agendado para: {nextSend}");
            Task.Delay(timeUntilNextSend).Wait();
        }
    }
}
