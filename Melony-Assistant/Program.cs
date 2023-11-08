using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;
using Timer = System.Timers.Timer;

public static class Program
{
    private static string[] toggleCommands = new string[]
    {
        "AirIn",
        "AirOut"
    };

    private static string domain;
    private static int port;

    private static string baseUrl { get { return $@"http://{domain}:{port}/"; } }
    private static Dictionary<string, string> toyIdToType;

    private static bool isOn = true;
    private static ulong userId = 0;
    public static void Main(string[] args)
    {
        
        DatabaseManager.Initialize();

        string json = SendPost("https://api.lovense.com/api/lan/getToys");
        var stringList = GetCellBy(json, "domain");

        if (json == "{}" || stringList.Count <= 0)
        {
            Console.WriteLine("No toys were found try making sure your phone isn't on standby!");
            return;
        }

        domain = stringList.First();
        string portStr = GetCellBy(json, "httpPort").First();
        if (!int.TryParse(portStr, out port))
        {
            Console.WriteLine("Error port string not a valid number!");
            Console.WriteLine(portStr);
            return;
        }

        string toyJson = GetCellBy(json, "toyJson").First();

        toyIdToType = new Dictionary<string, string>();
        List<string> ids = GetCellBy(toyJson, "id");

        var root = (JContainer)JToken.Parse(toyJson);
        var list = root.DescendantsAndSelf()
            .OfType<JProperty>()
            .Where(p => p.Name == "name");

        List<string> testy = new List<string>();
        foreach (var i in list)
        {
            var id = i.Parent.DescendantsAndSelf()
                .OfType<JProperty>()
                .Where(p => p.Name == "id")
                .Select(p => p.Value.Value<string>())
                .ToList().First();

            Console.WriteLine("FOUND TOY " + id + ":" + i.Value.ToString());
            toyIdToType.Add(id, i.Value.ToString());
        }

        string str = File.ReadAllText("UserId.txt");
        if(!ulong.TryParse(str, out userId))
        {
            Console.WriteLine("User Id is not a valid number!");
            return;
        }

        Timer t = new Timer();
        t.Interval = 100;
        t.Elapsed += OnTick;
        t.AutoReset = true;
        t.Start();

        for(; ; ) { }
    }

    static bool isProcessing = false;
    static long lastStamp = 0;

    private static void OnTick(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (isProcessing)
            return;

        isProcessing = true;
        string sql = "SELECT * FROM SessionParticipants WHERE participantId = " + userId;
        MySqlCommand cmd = new MySqlCommand(sql, DatabaseManager.conn);
        MySqlDataReader reader = cmd.ExecuteReader();

        if (!reader.HasRows)
        {
            cmd.Dispose();
            reader.DisposeAsync();
            return;
        }

        reader.Read();
        int sessionId = reader.GetInt32(1);
        cmd.Dispose();
        reader.DisposeAsync();

        sql = "SELECT * FROM Triggers WHERE sessionId = " + sessionId;
        cmd = new MySqlCommand(sql, DatabaseManager.conn);
        reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string word = reader.GetString(2);
            if (word.ToLower() != "nill")
                continue;

            string command = reader.GetString(3);
            string toy = reader.GetString(4);
            long stamp = reader.GetInt64(5);
            int value = reader.GetInt32(6);

            if(stamp > lastStamp)
            {
                Console.WriteLine("Execute " + command + " on " + toy + " with value " + value);

                string suffix = "?v=" + value;
                if (toggleCommands.Contains(command))
                    suffix = string.Empty;

                SendPost(baseUrl + (command.Replace("_", string.Empty)) + suffix);
                lastStamp = stamp;
            }
        }

        cmd.Dispose();
        reader.DisposeAsync();
        isProcessing = false;
    }

    private static string SendPost(string uri)
    {
        var client = new HttpClient();
        var webRequest = new HttpRequestMessage(HttpMethod.Post, uri);
        var response = client.Send(webRequest);
        using var reader = new StreamReader(response.Content.ReadAsStream());
        return reader.ReadToEnd();
    }

    private static List<string> GetCellBy(string json, string name)
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