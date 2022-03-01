using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonManager : MonoBehaviour
{
    public static AbilityButtonManager instance;

    [SerializeField]
    AbilityButton ability1;

    [SerializeField]
    AbilityButton ability2;

    [SerializeField]
    AbilityButton ability3;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
