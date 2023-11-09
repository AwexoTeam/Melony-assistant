using MelonyLovense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ToyCommand : Packet
{
    

    public LovenseCommand command;
    public int value;
    
    public ToyCommand() { }

    public ToyCommand(LovenseCommand command, int value, int time)
    {
        this.command = command;
        this.value = value;
    }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write((int)command);
        writer.Write((int)value);
        EndWrite();
    }

    public override void Deserialize(BinaryReader reader)
    {
        command = (LovenseCommand)reader.ReadInt32();
        value = reader.ReadInt32();
    }

    public override void OnRecieve(byte[] msg)
    {
        base.OnRecieve(msg);
        LovenseManager.SetDefault(command, value);
    }

    public override PacketType GetType() => PacketType.ToyCommand;
}