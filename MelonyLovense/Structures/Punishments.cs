using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Punishments
{
    public LovenseCommand cmd;
    public int value;
    public int ticks;

    public Punishments(LovenseCommand cmd, int value, int ticks)
    {
        this.cmd = cmd;
        this.value = value;
        this.ticks = ticks;
    }
}