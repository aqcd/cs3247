using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillJoystickManager : MonoBehaviour
{

    private SkillJoystickController skillJoystickController1;
    private SkillJoystickController skillJoystickController2;
    private SkillJoystickController skillJoystickController3;


    // Start is called before the first frame update
    void Start()
    {
        skillJoystickController1 = transform.GetChild(2).gameObject.GetComponent<SkillJoystickController>();
        skillJoystickController2 = transform.GetChild(3).gameObject.GetComponent<SkillJoystickController>();
        skillJoystickController3 = transform.GetChild(4).gameObject.GetComponent<SkillJoystickController>();

        skillJoystickController1.skillInUseEvent.AddListener(skill1InUse);
        skillJoystickController1.skillEndUseEvent.AddListener(skill1EndUse);

        skillJoystickController2.skillInUseEvent.AddListener(skill2InUse);
        skillJoystickController2.skillEndUseEvent.AddListener(skill2EndUse);

        skillJoystickController3.skillInUseEvent.AddListener(skill3InUse);
        skillJoystickController3.skillEndUseEvent.AddListener(skill3EndUse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void skill1InUse()
    {
        skillJoystickController2.disableJoystick();
        skillJoystickController3.disableJoystick();
    }

    public void skill1EndUse()
    {
        skillJoystickController2.enableJoystick();
        skillJoystickController3.enableJoystick();
    }

    public void skill2InUse()
    {
        skillJoystickController1.disableJoystick();
        skillJoystickController3.disableJoystick();
    }

    public void skill2EndUse()
    {
        skillJoystickController1.enableJoystick();
        skillJoystickController3.enableJoystick();
    }

    public void skill3InUse()
    {
        skillJoystickController1.disableJoystick();
        skillJoystickController2.disableJoystick();
    }

    public void skill3EndUse()
    {
        skillJoystickController1.enableJoystick();
        skillJoystickController2.enableJoystick();
    }
}
