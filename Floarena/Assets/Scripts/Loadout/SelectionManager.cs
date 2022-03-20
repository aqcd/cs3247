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

    public Dictionary<string, Sprite> itemSprites;
    public Dictionary<string, Sprite> skillSprites;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        itemSprites = new Dictionary<string, Sprite>();
        skillSprites = new Dictionary<string, Sprite>();
    }

    void Start() {
        // Load sprite resources
        // Load item sprites
        foreach (var item in Data.ITEMS) {
            Texture2D itemImg = Resources.Load("ItemIcons/" + item.name) as Texture2D;
            Sprite itemSprite = Sprite.Create(
                itemImg,
                new Rect(0f, 0f, itemImg.width, itemImg.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            itemSprites[item.name] = itemSprite;
        }

        // Load skill sprites
        foreach (var skill in Data.BASIC_SKILLS) {
            Texture2D skillImg = Resources.Load("SkillIcons/" + skill.name) as Texture2D;
            Sprite skillSprite = Sprite.Create(
                skillImg,
                new Rect(0f, 0f, skillImg.width, skillImg.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            skillSprites[skill.name] = skillSprite;
        }
        foreach (var skill in Data.ULTIMATE_SKILLS) {
            Texture2D skillImg = Resources.Load("SkillIcons/" + skill.name) as Texture2D;
            Sprite skillSprite = Sprite.Create(
                skillImg,
                new Rect(0f, 0f, skillImg.width, skillImg.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            skillSprites[skill.name] = skillSprite;
        }
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