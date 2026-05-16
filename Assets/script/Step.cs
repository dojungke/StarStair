using NUnit.Framework.Constraints;
using UnityEngine;

public abstract class Step
{
    public Stair stair;
    public string content;
    public abstract void ascend();
}
public class DummyStep : Step 
{
    public override void ascend()
    {
        stair.nowStep += 1;
        string[] contents = content.Split("/");
        switch (contents[1]) 
        {
            case "None":
                break;
            case "STR":
                if (stair.unit.STRength > int.Parse(contents[2]))
                    stair.ascend();
                break;
            case "HP":
                if (stair.unit.maxHP > int.Parse(contents[2]))
                    stair.ascend();
                break;
            case "Dex":
                if (stair.unit.DEXterity > int.Parse(contents[2]))
                    stair.ascend();
                break;
            case "INT":
                if(stair.unit.INTelligence > int.Parse(contents[2]))
                    stair.ascend();
                break;
        }
    }
}