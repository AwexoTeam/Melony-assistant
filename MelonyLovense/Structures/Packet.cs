using MelonyLovense;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Telepathy;

public partial class Packet
{
    public int id = -1;
    public byte[] buffer;
    public BinaryWriter writer;

    private MemoryStream stream;

    public Packet() { }
    public Packet(byte[] data)
    {
        buffer = data;
    }

    public void BeginWrite()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);

        writer.Write((int)GetType());
        writer.Write(id);
    }
    public void EndWrite()
    {
        buffer = stream.ToArray();
        stream.Close();
    }

    public virtual void Serialize() { }
    public virtual void Deserialize(BinaryReader reader) { }

    public virtual void OnRecieve(byte[] msg) { }

    public void Send()
    {
        Serialize();
        ServerManager.client.Send(buffer);
    }

    public virtual PacketType GetType() => PacketType.Unknown;
}