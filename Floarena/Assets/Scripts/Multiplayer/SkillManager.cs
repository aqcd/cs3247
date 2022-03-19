using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    
    public static SkillManager instance;
    public List<Skill> skillList;
    public List<GameObject> skillObjs;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        skillList = new List<Skill>();
        skillObjs = new List<GameObject>();
    }

    void Start() {}

    void Update() {}

    public void LoadSkills(Skill[] loadoutSkills) {
        if (skillObjs.Count > 0) {
            UnLoadSkills();
        }

        for (int i = 0; i < loadoutSkills.Length; i++) {
            skillList.Add(loadoutSkills[i]);
            StartCoroutine(LoadSkillsCoroutine(loadoutSkills[i].ToString()));
        }
    }

    public void UnLoadSkills() {
        skillList.Clear();
        foreach (var obj in skillObjs) {
            Resources.UnloadAsset(obj);
        }
        Debug.Log("Old resources unloaded");
    }

    IEnumerator LoadSkillsCoroutine(string skillName) {
        Debug.Log("Loading " + skillName);
        ResourceRequest req = Resources.LoadAsync("Skills/" + skillName);

        while (!req.isDone) {
            yield return null;
        }

        skillObjs.Add(req.asset as GameObject);
        Debug.Log("Done loading " + skillName);
    }
}
