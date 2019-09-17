
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    public string unit_name;
    public float max_hp;
    public bool shield = false;//щит
    public bool stun = false;//оглушение
    public float poison = 2;//яд
    public List<Ability> abilities = new List<Ability>();

    float _current_hp;
    public float current_hp
    {
        get
        {
            return _current_hp;
        }
        set
        {
            _current_hp = value;
            if (_current_hp < 0.0f)
                _current_hp = 0.0f;
            if (_current_hp > max_hp)
                _current_hp = max_hp;
        }
    }
    List<Ability> available_abilities = new List<Ability>();

    public void ApplyAbility(Ability ability)
    {
        //print(unit_name + " gets " + shield.ToString());
        ability.Use();
        //атака
        if(ability.damage > 0)
            if (shield)
                current_hp -= ability.damage / 2;
            else
                current_hp -= ability.damage;
        else
            current_hp -= ability.damage;//лечилка
        if (ability.stun)
            stun = true;
        if (ability.poison>0)
            poison += ability.poison;
        if (ability.shield)
            shield = true;

    }

    public void Start()
    {
        for (int i = 0; i < abilities.Count; ++i)
            abilities[i] = GameObject.Instantiate(abilities[i]);
        current_hp = max_hp;
    }

    Ability GetRandomAbility()
    {
        available_abilities.Clear();
        foreach (var ability in abilities)
        {
            if (ability.is_ready)
                available_abilities.Add(ability);
        }
        return available_abilities[UnityEngine.Random.Range(0, available_abilities.Count)];
    }

    public void OnTurnStart()
    {
        foreach (var ability in abilities)
            ability.OnTurnStart();
    }
}
