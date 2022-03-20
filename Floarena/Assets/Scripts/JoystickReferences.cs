using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickReferences : MonoBehaviour {
    public static JoystickReferences instance;
    public List<SkillJoystickController> skillJoysticks;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
}
