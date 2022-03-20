using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {
    
    public static SkillManager instance;
    public List<Skill> skillList;
    public List<GameObject> skillObjs;
    public List<Sprite> skillImgs;


    public List<SkillJoystickController> skillJoysticks;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        skillList = new List<Skill>();
        skillObjs = new List<GameObject>();
        skillImgs = new List<Sprite>();
    }

    void Start() {}

    void Update() {
        // if (Input.GetKeyDown(KeyCode.W)) {
        //     Debug.Log(skillObjs[1]);
        //     GameObject go = Instantiate(skillObjs[1]);
        //     go.SendMessage("Execute", new Vector3(1.0f, 0.0f, 0.0f));
        // }
    }

    public void LoadSkills(Skill[] loadoutSkills) {
        if (skillObjs.Count > 0) {
            UnLoadSkills();
        }

        for (int i = 0; i < loadoutSkills.Length; i++) {
            skillList.Add(loadoutSkills[i]);
            // StartCoroutine(LoadSkillsCoroutine(loadoutSkills[i].ToString()));

            // Load skills
            GameObject skillObjPrefab = Resources.Load("Skills/" + loadoutSkills[i].ToString()) as GameObject;
            GameObject skillObj = Instantiate(skillObjPrefab);
            skillObj.transform.parent = transform;
            skillObjs.Add(skillObj);

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

        for (int i = 0; i < skillJoysticks.Count; i++) {
            skillJoysticks[i].SetSkill(skillList[i]);
            skillJoysticks[i].SetSkillObject(skillObjs[i]);
            skillJoysticks[i].SetSkillImage(skillImgs[i]);
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

    /*
    IEnumerator LoadSkillsCoroutine(string skillName) {
        Debug.Log("Loading " + skillName);
        ResourceRequest req = Resources.LoadAsync("Skills/" + skillName);

        while (!req.isDone) {
            yield return null;
        }

        skillObjs.Add(req.asset as GameObject);
        Debug.Log("Done loading " + skillName);
    }
    */

    public Skill GetSkill(int index) {
        return skillList[index];
    }

    public GameObject GetSkillObject(int index) {
        return skillObjs[index];
    }
}
