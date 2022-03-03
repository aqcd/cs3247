using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


    public class ThirdPersonDash : MonoBehaviour
    {
        public ThirdPersonController moveScript;

        public float dashSpeed;
        public float dashTime;
    
        private float targetRotation;

        // Start is called before the first frame update
        void Start()
        {
            moveScript = GetComponent<ThirdPersonController>();
        
        }

        // Update is called once per frame
        void Update()
        {
            if (moveScript._input.dash)
            {
                StartCoroutine(Dash());
                moveScript._input.dash = false;
            }
        }

        IEnumerator Dash()
        {
            float startTime = Time.time;

            Vector3 inputDirection = new Vector3(moveScript._input.move.x, 0.0f, moveScript._input.move.y).normalized;
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + moveScript._mainCamera.transform.eulerAngles.y;
		    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref moveScript._rotationVelocity, moveScript.RotationSmoothTime);

		    // rotate to face input direction relative to camera position
		    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            while (Time.time < startTime + dashTime)
            {
                moveScript._controller.Move(targetDirection.normalized * dashSpeed * Time.deltaTime);

                yield return null;
            }

        }
    }
