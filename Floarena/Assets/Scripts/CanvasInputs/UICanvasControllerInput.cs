using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        // public void VirtualLookInput(Vector2 virtualLookDirection)
        // {
        //     starterAssetsInputs.LookInput(virtualLookDirection);
        // }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }

        public void VirtualDashInput(bool virtualDashState)
        {
            starterAssetsInputs.DashInput(virtualDashState);
        }

        public void VirtualSkillshotInput(Vector2 virtualSkillshotDirection)
        {
            starterAssetsInputs.SkillshotInput(virtualSkillshotDirection);
        }

        public void VirtualSkillshotStateInput(bool virtualSkillshotState)
        {
            starterAssetsInputs.SkillShotStateInput(virtualSkillshotState);
        }

        public void VirtualTargetCircleInput(Vector2 virtualTargetCircleDirection)
        {
            starterAssetsInputs.TargetCircleInput(virtualTargetCircleDirection);
        }

        public void VirtualTargetCircleState(bool virtualTargetCircleState)
        {
            starterAssetsInputs.TargetCircleStateInput(virtualTargetCircleState);
        }
        
    }

}
