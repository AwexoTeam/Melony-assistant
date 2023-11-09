using MelonyLovense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telepathy;

public static class ServerManager
{
    public static Client client;
    public static DateTime timeStamp = DateTime.MinValue;
    public static Dictionary<PacketType, Packet> packets = new Dictionary<PacketType, Packet>();

    private static MemoryStream stream;
    private static BinaryReader reader;

    public static void Initialize()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(Packet)));

        foreach (var type in types)
        {
            Packet packet = (Packet)Activator.CreateInstance(type);
            if (packets.ContainsKey(packet.GetType()))
                continue;

            packets.Add(packet.GetType(), packet);
        }

    }

    public static void Tick()
    {
        client.Tick(100);
    }

    public static bool ConnectToServer()
    {
        if (client != null && client.Connected)
            return true;

        if (timeStamp == DateTime.MinValue)
        {
            client = new Client(999);
            client.Connect("localhost", 30040);
            client.OnData += OnRecieved;
            client.OnConnected += OnConnected;
            timeStamp = DateTime.Now;
            return false;
        }
        else
        {
            if (client.Connected)
                return true;

            var span = DateTime.Now - timeStamp;
            if (span.TotalSeconds < 5)
            {
                Form1.instance.SetStatus("Connecting...", Color.Yellow);
                return false;
            }
            else
            {
                if (!client.Connected)
                {
                    timeStamp = DateTime.MinValue;
                    return false;
                }
            }

        }


        return true;
    }

    private static void OnConnected()
    {
        RegisterUser register = new RegisterUser(Form1.instance.userId);
        register.Send();
    }

    private static void OnRecieved(ArraySegment<byte> data)
    {
        stream = new MemoryStream(data.Array);
        reader = new BinaryReader(stream);

        PacketType type = (PacketType)reader.ReadInt32();
        int id = reader.ReadInt32();

        if (packets.ContainsKey(type))
        {
            Console.WriteLine("Recieved packet " + type);
            Packet packet = packets[type];
            packet.Deserialize(reader);
            packet.OnRecieve(data.Array);
        }
    }

}