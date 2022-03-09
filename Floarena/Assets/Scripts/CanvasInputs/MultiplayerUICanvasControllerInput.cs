using UnityEngine;

public class MultiplayerUICanvasControllerInput : MonoBehaviour
{

    [Header("Output")]
    public MultiplayerInputs multiplayerInputs;

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        multiplayerInputs.MoveInput(virtualMoveDirection);
    }

    public void AttachMultiplayerInputs(MultiplayerInputs inputs)
    {
        multiplayerInputs = inputs;
    }
}

