using UnityEngine;
using Mirror;
public class ADBerry : MonoBehaviour {
  private MapVisualizer mapVisualizer;
  private float buffMagnitude = PickupConstants.AD_BERRY_MAGNITUDE;
  private float buffDuration = PickupConstants.AD_BERRY_DURATION;

  private Effect buffEffect;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
    buffEffect = new Effect(Attribute.AD, buffMagnitude);
  }
  private void OnTriggerEnter(Collider collider) 
  {
    if (collider.gameObject.GetComponent<MultiplayerThirdPersonController>().isLocalPlayer) {
        collider.gameObject.GetComponent<PlayerManager>().BuffForDuration(buffEffect, buffDuration);
    }
    Vector3 positionOnGrid = transform.position;
    mapVisualizer.SpawnPickupItem(positionOnGrid);
    Destroy(gameObject);
  }
}