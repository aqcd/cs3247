using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;

    public bool isItemSelected = false;
    public bool isBSkillSelected = false;
    public bool isUSkillSelected = false;
    public int editingPosition = 1;

    [SerializeField]
    public ItemSelection[] itemSelections;

    [SerializeField]
    public SkillSelection[] skillSelections;

    [SerializeField]
    public ItemSpriteDictionary itemSprites;

    [SerializeField]
    public SkillSpriteDictionary skillSprites;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        
    }

    public void SetEditingPosition(int position) {
        this.isItemSelected = false;
        this.isBSkillSelected = false;
        this.isUSkillSelected = false;
        this.editingPosition = position;

        if (position == 1 || position == 2) {
            this.isItemSelected = true;
        } else if (position == 3 || position == 4) {
            this.isBSkillSelected = true;
        } else {
            this.isUSkillSelected = true;
        }
    }

    public void SetItem(Item item) {
        if (this.isItemSelected && LoadoutManager.instance.SetItem(item, editingPosition)) {
            this.itemSelections[editingPosition - 1].SetItem(item);
            this.isItemSelected = false;
        }
        EditLoadoutUIManager.instance.UnsetDescription();
    }

    public void SetSkill(Skill skill) {
        int skillPosition = editingPosition - 2;
        if ((this.isBSkillSelected || this.isUSkillSelected) && LoadoutManager.instance.SetSkill(skill, skillPosition)) {
            this.skillSelections[skillPosition - 1].SetSkill(skill);
            this.isBSkillSelected = false;
            this.isUSkillSelected = false;
        }
        EditLoadoutUIManager.instance.UnsetDescription();
    }

    public void SetFromLoadout(Loadout loadout) {
        for (int i = 0; i < itemSelections.Length; i++) {
            this.itemSelections[i].SetItem(loadout.items[i]);
        }
        for (int i = 0; i < skillSelections.Length; i++) {
            this.skillSelections[i].SetSkill(loadout.skills[i+1]);
        }
        this.isItemSelected = false;
        this.isBSkillSelected = false;
        this.isUSkillSelected = false;
    }
}