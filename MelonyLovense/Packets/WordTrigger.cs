using MelonyLovense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WordTrigger : Packet
{
    public LovenseCommand command;
    public int value;
    public int time;

    public WordTrigger() { }

    public WordTrigger(LovenseCommand command, int value, int time)
    {
        this.command = command;
        this.value = value;
        this.time = time;
    }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write((int)command);
        writer.Write((int)value);
        writer.Write((int)time);
        EndWrite();
    }

    public override void Deserialize(BinaryReader reader)
    {
        command = (LovenseCommand)reader.ReadInt32();
        value = reader.ReadInt32();
        time = reader.ReadInt32();
    }

    public override void OnRecieve(byte[] msg)
    {
        LovenseManager.AddPunishment(command, value, time);
    }

    public override PacketType GetType() => PacketType.WordTrigger;
}