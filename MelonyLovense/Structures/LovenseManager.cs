using MelonyLovense;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class LovenseManager
{
    public static string baseUrl { get { return $@"http://{domain}:{port}/"; } }

    public static Dictionary<string, string> toyIdToType;

    public static string domain;
    public static int port;
    public static Dictionary<LovenseCommand, int> currerntSpeeds;
    public static Dictionary<LovenseCommand, int> lastValues;
    public static List<Punishments> currentPunishments;
    private static LovenseCommand[] lovenseCommands;

    public static string[] toggleCommands = new string[]
    {
            "AirIn",
            "AirOut"
    };

    public static bool TryConnect()
    {
        string json = Utils.SendPost("https://api.lovense.com/api/lan/getToys");
        var stringList = Utils.GetCellBy(json, "domain");

        if (json == "{}" || stringList.Count <= 0)
        {
            Form1.instance.SetError("No toys. Is Lovense Connect on?");
            return false;
        }

        domain = stringList.First();
        string portStr = Utils.GetCellBy(json, "httpPort").First();
        if (!int.TryParse(portStr, out port))
        {
            Form1.instance.SetError("Error port string not a valid number! (" + portStr + ")");
            return false;
        }

        string toyJson = Utils.GetCellBy(json, "toyJson").First();

        toyIdToType = new Dictionary<string, string>();
        List<string> ids = Utils.GetCellBy(toyJson, "id");

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

            Form1.instance.SetError("FOUND TOY " + id + ":" + i.Value.ToString());
            toyIdToType.Add(id, i.Value.ToString());
        }

        return true;
    }

    public static void Initialize()
    {
        currerntSpeeds = new Dictionary<LovenseCommand, int>();
        lastValues = new Dictionary<LovenseCommand, int>();
        currentPunishments = new List<Punishments>();
        toyIdToType = new Dictionary<string, string>();

        lovenseCommands = Enum.GetValues(typeof(LovenseCommand)).Cast<LovenseCommand>().ToArray();
        foreach (var c in lovenseCommands)
        {
            currerntSpeeds.Add(c, 0);
            lastValues.Add(c, 0);
        }
    }

    public static void Tick()
    {
        foreach (var c in lovenseCommands)
        {
            var punishment = currentPunishments.Find(x => x.cmd == c);
            if(punishment != null)
            {
                BuzzToy(c, punishment.value);

                punishment.ticks--;
                if(punishment.ticks <= 0)
                    currentPunishments.Remove(punishment);

                continue;
            }
            BuzzToy(c, currerntSpeeds[c]);
        }
    }

    public static void BuzzToy(LovenseCommand command, int value)
    {
        if (value == lastValues[command])
            return;

        string suffix = "?v=" + value;
        if (toggleCommands.Contains(command.ToString()))
            suffix = string.Empty;

        Console.WriteLine("Buzzing " + command + " for " + value);
        Utils.SendPost(baseUrl + (command.ToString().Replace("_", string.Empty)) + suffix);
        lastValues[command] = value;
    }

    public static void SetDefault(LovenseCommand command, int value)
    {
        currerntSpeeds[command] = value;
    }

    internal static void AddPunishment(LovenseCommand c, int value, int time)
    {
        Console.WriteLine("Adding punishment!");
        var punishment = currentPunishments.Find(x => x.cmd == c);
        if (punishment == null)
        {
            punishment = new Punishments(c, value, time);
            currentPunishments.Add(punishment);
            return;
        }

        punishment.ticks += time;
    }
}