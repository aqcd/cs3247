using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField]
	private CharacterController _controller;

    [SerializeField]
    private MultiplayerThirdPersonController _mcontroller;

    private float cd = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cd);
        cd -= Time.deltaTime;
        if (cd < 0.0f) {
            if (_mcontroller.disablePlayerMove) {
                _mcontroller.disablePlayerMove = false;
            }
            if (cd < -5.0f) {
                cd = 0.5f;
            }
            return;
        }
        if (!_mcontroller.disablePlayerMove) {
            _mcontroller.disablePlayerMove = true;
        }
        Vector3 targetDirection = new Vector3(1.0f, 0.0f, 1.0f);
        float _speed = 20.0f;
        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));
    }
}
