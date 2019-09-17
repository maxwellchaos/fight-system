using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class UnitUI : MonoBehaviour
{
    Unit _unit;
    public Unit unit
    {
        set
        {
            _unit = value;
            prev_hp = _unit.current_hp;
        }
        get
        {
            return _unit;
        }
    }

    new public Text name;
    public Text hp;
    public Text popup;

    public float prev_hp = -1;
    public bool stun = false;

    void Update()
    {
        if (unit == null)
            return;

        if (prev_hp != unit.current_hp)
        {
            if(prev_hp - unit.current_hp == unit.poison)
                ShowAct("-"+unit.poison.ToString()+"(poisoned)");
            else
            ShowDamage(prev_hp - unit.current_hp);
            prev_hp = unit.current_hp;
        }
        if (unit.stun && !stun)
        {
            stun = true;
            ShowAct("stuned");
        }
        if (unit.current_hp <= 0.0f)
            gameObject.SetActive(false);

        name.text = unit.unit_name;
        hp.text = $"HP: {unit.current_hp}/{unit.max_hp}";
    }

    void ShowDamage(float damage)
    {
        var new_popup = Text.Instantiate(popup, popup.transform.parent);
        new_popup.text = $"{-damage}";
        if (damage > 0)
            new_popup.color = Color.red;
        else
            new_popup.color = Color.green;
        new_popup.gameObject.AddComponent<PopupText>();
        new_popup.gameObject.SetActive(true);
    }
    void ShowAct(string damage)
    {
        var new_popup = Text.Instantiate(popup, popup.transform.parent);
        new_popup.text = damage;

        new_popup.color = Color.red;

        new_popup.gameObject.AddComponent<PopupText>();
        new_popup.gameObject.SetActive(true);
    }
}