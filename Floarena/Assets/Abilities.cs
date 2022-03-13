using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class Abilities : MonoBehaviour
{

    private StarterAssetsInputs inputs;
    private GameObject mainCamera;
    
    private float rotationVelocity;

    public float RotationSmoothTime = 0.12f;

    [Header("Ability 1")]
    public Image ability1Image;
    public float cooldown1;
    bool isCooldown1 = false;
    

    Vector3 position;
    public Canvas ability1Canvas;
    public Image skillshot;
    public Transform player;


    [Header("Ability 2")]
    public Image ability2Image;
    public float cooldown2;
    bool isCooldown2 = false;

    public Image targetCircle;
    public Image indicatorRangeCircle;
    public Canvas ability2Canvas;
    private Vector3 posUp;
    public float maxAbility2Distance;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
        ability1Image.fillAmount = 0;
        ability2Image.fillAmount = 0;

        skillshot.GetComponent<Image>().enabled = false;
        targetCircle.GetComponent<Image>().enabled = false;
        indicatorRangeCircle.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
        Ability2();
        Ability3();


        //Ability 1 Input
        position = new Vector3(inputs.skillshot.x, 0.0f, inputs.skillshot.y).normalized;

        //Ability 2 Input
        posUp = new Vector3(inputs.targetCircle.x, 10f, inputs.targetCircle.y).normalized;
        position = new Vector3(inputs.targetCircle.x, 0.0f, inputs.targetCircle.y).normalized;


        //Ability 1 Canvas Input
        float targetRotation = Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
        ability1Canvas.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        // Quatenion transRot = Quatenion.LookRotation(position - player.transform.position);
        // ability1Canvas.transform.rotation = Quatenion.Lerp(transRot, ability1Canvas.transform.rotation, 0f);

        //Ability 2 Canvas Input
        // var hitPosDir = (position - transform.position).normalized;
        // float distance = Vector3.Distance(position, transform.position);
        // distance = Mathf.Min(distance, maxAbility2Distance);

        // var newHitPos = transform.position + hitPosDir + distance;
        // ability2Canvas.transform.position = (newHitPos);

    }

    void Ability1()
    {
        if (inputs.skillshotPressed && isCooldown1 == false)
        {
            skillshot.GetComponent<Image>().enabled = true;

            indicatorRangeCircle.GetComponent<Image>().enabled = false;
            targetCircle.GetComponent<Image>().enabled = false;
            
        }

        // if (skillshot.GetComponent<Image>().enabled = true && !inputs.skillshotPressed)
        // {
        //     isCooldown1 = false;
        //     ability1Image.fillAmount = 1;
        // }

        // if (isCooldown1)
        // {
        //     ability1Image.fillAmount -= 1 / cooldown1 * Time.deltaTime;
        //     skillshot.GetComponent<Image>().enabled = false;

        //     if (ability1Image.fillAmount <= 0)
        //     {
        //         ability1Image.fillAmount = 0;
        //         isCooldown1 = false;
        //     }
        // }
    }


    void Ability2()
    {
        if (inputs.targetCirclePressed && isCooldown2 == false)
        {
            indicatorRangeCircle.GetComponent<Image>().enabled = true;
            targetCircle.GetComponent<Image>().enabled = true;

            skillshot.GetComponent<Image>().enabled = false;

            isCooldown2 = true;
            ability2Image.fillAmount = 1;
        }

        // if (targetCircle.GetComponent<Image>().enabled = true && !inputs.targetCirclePressed)
        // {
        //     isCooldown2 = true;
        //     ability2Image.fillAmount = 1;
        // }

        // if (isCooldown2)
        // {
        //     ability2Image.fillAmount -= 1 / cooldown2 * Time.deltaTime;

        //     indicatorRangeCircle.GetComponent<Image>().enabled = false;
        //     targetCircle.GetComponent<Image>().enabled = false;

        //     if (ability2Image.fillAmount <= 0)
        //     {
        //         ability2Image.fillAmount = 0;
        //         isCooldown2 = false;
        //     }
        // }
    }

    void Ability3()
    {

    }
}
