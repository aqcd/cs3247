using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SkillManager : NetworkBehaviour {
    
    public static SkillManager instance;
    public List<Skill> skillList;
    public List<GameObject> skillObjs;
    public List<Sprite> skillImgs;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        skillList = new List<Skill>();
        skillObjs = new List<GameObject>();
        skillImgs = new List<Sprite>();
    }
    

    [ClientRpc]
    public void LoadSkills() {
        // if (skillObjs.Count > 0) {
        //     UnLoadSkills();
        // }

        Skill[] loadoutSkills = GameManager.instance.loadout.skills;
        string[] skillNames = new string[loadoutSkills.Length];

        for (int i = 0; i < loadoutSkills.Length; i++) {
            skillList.Add(loadoutSkills[i]);
            skillNames[i] = loadoutSkills[i].name;

            // Load skill object prefab and register it on the server
            GameObject skillObjPrefab = Resources.Load("Skills/" + skillNames[i]) as GameObject;
            NetworkClient.RegisterPrefab(skillObjPrefab);

            // Load projectile prefabs for each skill
            foreach (var projectile in skillObjPrefab.GetComponent<ProjectileManager>().projectilePrefabs) {
                NetworkClient.RegisterPrefab(projectile);
            }
            
            // Load skill images
            Texture2D skillImg = Resources.Load("SkillIcons/" + loadoutSkills[i].ToString()) as Texture2D;
            Sprite skillSprite = Sprite.Create(
                skillImg, 
                new Rect(0f, 0f, skillImg.width, skillImg.height), 
                new Vector2(0.5f, 0.5f),
                100f
            );
            skillImgs.Add(skillSprite);
        }

        // Load the requested skills on the server
        Debug.Log("Player " + MatchManager.instance.GetPlayerNum() + " asking server to load skills");
        ServerLoadSkills(MatchManager.instance.GetPlayerNum(), skillNames);

        // Set skills 
        List<SkillJoystickController> skillJoysticks = JoystickReferences.instance.skillJoysticks;
        for (int i = 0; i < skillJoysticks.Count; i++) {
            skillJoysticks[i].SetSkill(skillList[i]);
            skillJoysticks[i].SetSkillImage(skillImgs[i]);
        }
    }

    [Command(requiresAuthority=false)]
    private void ServerLoadSkills(int targetPlayer, string[] skillNames) {
        Debug.Log("called by " + targetPlayer);
        
        // Load skills
        for (int i = 0; i < skillNames.Length; i++) {
            GameObject skillObjPrefab = Resources.Load("Skills/" + skillNames[i]) as GameObject;
            GameObject skillObj = Instantiate(skillObjPrefab);
            skillObj.transform.parent = transform;
            NetworkServer.Spawn(skillObj);
            SetSkillObject(targetPlayer, skillObj, i);
        }
    }

    [ClientRpc]
    private void SetSkillObject(int targetPlayer, GameObject skillObj, int index) {
        if (MatchManager.instance.GetPlayerNum() == targetPlayer) {
            Debug.Log("Player " + targetPlayer + " setting skills");
            List<SkillJoystickController> skillJoysticks = JoystickReferences.instance.skillJoysticks;
            skillJoysticks[index].SetSkillObject(skillObj);
            Debug.Log("Object id: " + skillObj.GetComponent<NetworkIdentity>().netId);
        }
    }

    public void UnLoadSkills() {
        skillList.Clear();
        foreach (var obj in skillObjs) {
            Resources.UnloadAsset(obj);
        }
        foreach (var obj in skillImgs) {
            Resources.UnloadAsset(obj);
        }
        Debug.Log("Old resources unloaded");
    }

    

    public Skill GetSkill(int index) {
        return skillList[index];
    }

    public GameObject GetSkillObject(int index) {
        return skillObjs[index];
    }
}
