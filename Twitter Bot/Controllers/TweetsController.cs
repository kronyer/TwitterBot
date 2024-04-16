using Microsoft.AspNetCore.Mvc;
using System.Text;
using Tweetinvi;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Models;
using Twitter_Bot.Models;

namespace Twitter_Bot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : Controller
    {
        private readonly IConfiguration _configuration;

        public TweetsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> PostTweet(PostTweetRequest newTweet)
        {
            var apiKey = _configuration["AppSettings:TwitterApiKey"];
            var apiKeySecret = _configuration["AppSettings:TwitterApiSecret"];
            var accessToken = _configuration["AppSettings:TwitterAccessToken"];
            var accessTokenSecret = _configuration["AppSettings:TwitterAccessTokenSecret"];

            var client = new TwitterClient(apiKey, apiKeySecret, accessToken, accessTokenSecret);


            var result = await client.Execute.AdvanceRequestAsync(BuildTwitterRequest(newTweet, client));

            return Ok(result.Content);
        }

        private static Action<ITwitterRequest> BuildTwitterRequest(PostTweetRequest newTweet, TwitterClient client)
        {
            return (ITwitterRequest request) =>
            {
                var jsonBody = client.Json.Serialize(newTweet);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                request.Query.Url = "https://api.twitter.com/2/tweets";
                request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                request.Query.HttpContent = content;

            };
        }
    }
}
