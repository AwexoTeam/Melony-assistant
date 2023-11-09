using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Utils
{
    public static string SendPost(string uri)
    {
        var client = new HttpClient();
        var webRequest = new HttpRequestMessage(HttpMethod.Post, uri);
        var response = client.Send(webRequest);
        using var reader = new StreamReader(response.Content.ReadAsStream());
        return reader.ReadToEnd();
    }

    public static List<string> GetCellBy(string json, string name)
    {
        var root = (JContainer)JToken.Parse(json);

        List<string>? list = new List<string>();
        list = root.DescendantsAndSelf()
            .OfType<JProperty>()
            .Where(p => p.Name == name)
            .Select(p => p.Value.Value<string>())
            .ToList();

        return list;
    }

}