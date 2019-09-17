using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability Data", order = 1)]
public class Ability : ScriptableObject
{
    new public string name;
    public float damage;
    public uint cooldown;
    public bool shield = false;//щит
    public bool stun = false;//оглушение
    public float poison = 0;//яд
    public bool toSelf;//действует на себя

    [NonSerialized]
    public uint recharge_timer;

    public bool is_ready
    {
        get
        {
            return recharge_timer == 0;
        }
    }
    public float calcPriority()
    {
        float damage = this.damage;
        if (this.stun)
            damage += 5f;
        if (this.poison>0.0f)
            damage += 10f;
        return damage;
    }

    public bool prepare { get; set; }

    public float Use()
    {
        prepare = false;
        recharge_timer = cooldown;
        return damage;
    }

    public void OnTurnStart()
    {
        if (recharge_timer > 0)
            recharge_timer--;
    }
}
