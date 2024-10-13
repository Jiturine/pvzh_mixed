using System.Data;

public class NoCounterAttack : Ability
{
    public override void SetInfo()
    {
        name = "不反";
        description = "这名队友攻击敌方时，敌方不会反击";
        ID = 1;
    }
}
