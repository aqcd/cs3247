using UnityEngine;
using Mirror;
public class MSBerry : MonoBehaviour {
  private MapVisualizer mapVisualizer;
  private float buffMagnitude = BerryConstants.MS_BERRY_MAGNITUDE;
  private float buffDuration = BerryConstants.MS_BERRY_DURATION;

  private Effect buffEffect;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
    buffEffect = new Effect(Attribute.MS, buffMagnitude);
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