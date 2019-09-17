using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public AbilityUI ability_ui;
    public List<Unit> good_units = new List<Unit>();
    public List<Unit> evil_units = new List<Unit>();
    public List<Ability> allAbilities = new List<Ability>();

    public GameObject evil_units_ui;
    public GameObject good_units_ui;

    bool is_turn_end = true;
    bool is_battle_end
    {
        get
        {
            return good_units.Count == 0 || evil_units.Count == 0;
        }
    }

    void Start()
    {
        print(0);
        is_turn_end = true;
        InitUnitsUI(evil_units, evil_units_ui);
        InitUnitsUI(good_units, good_units_ui);
    }

    void MarkUnit(GameObject units_ui, Ability ability, bool check)
    {
        for (int i = 0; i < units_ui.transform.childCount; ++i)
        {
            var unit_ui = units_ui.transform.GetChild(i).gameObject;
            if (!unit_ui.activeSelf)
                continue;

            var unit_ui_button = unit_ui.GetComponent<Button>();
            unit_ui_button.interactable = check;
        }
    }

    void InitUnitsUI(List<Unit> units, GameObject units_ui)
    {
       
        Debug.Log("Init");
        int i;
        for (i = 0; i < units.Count; ++i)
        {
            int n1 = Random.Range(1, allAbilities.Count);
            int n2 = Random.Range(1, allAbilities.Count - 1);
            if (n2 >= n1)
                n2++;
            units[i].abilities[1] = allAbilities[n1];
            units[i].abilities[2] = allAbilities[n2];
            GameObject unit_ui;
            if (i < units_ui.transform.childCount)
                unit_ui = units_ui.transform.GetChild(i).gameObject;
            else
                unit_ui = GameObject.Instantiate(units_ui.transform.GetChild(0).gameObject, units_ui.transform);
            unit_ui.GetComponent<UnitUI>().unit = units[i];
            var unit = units[i];

            unit_ui.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                ability_ui.target = unit;
            });
        }

        for (; i < units_ui.transform.childCount; ++i)
            units_ui.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Update()
    {
        if (is_turn_end && !is_battle_end)
            StartCoroutine(Turn());
    }

    void OnAbilitySelected(Ability selected)
    {
        if (selected != null)
        {
            //print(selected.name);
            //print(selected.toSelf);
            MarkUnit(evil_units_ui, selected, !selected.toSelf);
            MarkUnit(good_units_ui, selected, selected.toSelf);

        }
    }

    public IEnumerator Turn()
    {
        is_turn_end = false;
        foreach (var unit in good_units)
        {
            unit.OnTurnStart();
            if (unit.stun)
                unit.stun = false;
            if (unit.poison > 0.0f)
                unit.current_hp -= unit.poison;
                
            ability_ui.gameObject.SetActive(true);
            yield return ability_ui.WaitInput(unit, good_units, evil_units, OnAbilitySelected);
            ability_ui.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);
        }
        RemoveDead();
        foreach (var unit in evil_units)
        {
            unit.OnTurnStart();
            if (unit.poison > 0)
                unit.current_hp -= unit.poison;
            if (unit.current_hp > 0.0f)
            {
                if (unit.stun)
                    unit.stun = false;
                if (unit.poison > 0.0f)
                    unit.current_hp -=unit.poison;
                else
                    AI.MakeAction(unit, evil_units, good_units);
            }
            yield return new WaitForSeconds(1.0f);
        }
        RemoveDead();
        is_turn_end = true;
        yield return null;
    }

    void RemoveDead()
    {
        good_units.RemoveAll(unit => unit.current_hp <= 0.0f);
        evil_units.RemoveAll(unit => unit.current_hp <= 0.0f);
    }
}
