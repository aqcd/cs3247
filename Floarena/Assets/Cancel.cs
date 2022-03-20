using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cancel : MonoBehaviour
{
    [SerializeField]
    public SkillJoystickController skillJoystickController1;
    public SkillJoystickController skillJoystickController2;
    public SkillJoystickController skillJoystickController3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            int id = touch.fingerId;
            if (EventSystem.current.IsPointerOverGameObject(id))
            {
                skillJoystickController1.isCancel = true;
                skillJoystickController2.isCancel = true;
                skillJoystickController3.isCancel = true;
                return;
            // ui touched
            }
        }

        skillJoystickController1.isCancel = false;
        skillJoystickController2.isCancel = false;
        skillJoystickController3.isCancel = false;
    }
}
