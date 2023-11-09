using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RegisterUser : Packet
{
    public ulong userId;

    public RegisterUser() { }
    public RegisterUser(ulong id) { userId = id; }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write(userId);
        EndWrite();
    }

    public override void Deserialize(BinaryReader reader)
    {
        userId = reader.ReadUInt64();
    }

    public override PacketType GetType() => PacketType.RegisterUser;
}