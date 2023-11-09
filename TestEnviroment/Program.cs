using System;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Client.Connectors.WebsocketConnector;
using Newtonsoft.Json;
using System.Text;

namespace AsyncExample
{
    public class AuthContent
    {
        public string token;
        public string uid;
        public string uname;
        public string utoken;

        public AuthContent(string token, string uid, string uname, string utoken)
        {
            this.token = token;
            this.uid = uid;
            this.uname = uname;
            this.utoken = utoken;
        }
    }

    class Program
    {
        private static async Task AwaitExample()
        {
            string uri = "https://api.lovense-api.com/api/basicApi/getToken";
            var client = new HttpClient();

            AuthContent auth = new AuthContent("o50gAW4m/YQ1tuMSwjYbDVCX4tZZ4j1//q/+lG4njz+lI04YfiLK7lMxzf078phE","carl1", "carl2", "carl3");
            string json = JsonConvert.SerializeObject(auth);
            await Console.Out.WriteLineAsync(json);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(json,Encoding.UTF8, "application/json");
            await client.SendAsync(request)
            .ContinueWith(responseTask =>
            {
                json = responseTask.Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Response: {0}", json);
            });

            //var content = new FormUrlEncodedContent(json);
            //var response = await client.PostAsync(uri, content);

            //var responseString = await response.Content.ReadAsStringAsync();
            //await Console.Out.WriteLineAsync(responseString);
        }

        private static void Main(string[] args)
        {
            AwaitExample().Wait();
        }
    }
}