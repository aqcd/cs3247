using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, ISkill
{
    private GameObject player;

    void Start()
    {
        player = GameManager.instance.GetPlayer();
    }

    public void Execute(Vector3 skillPosition) 
    {   
    }
}
