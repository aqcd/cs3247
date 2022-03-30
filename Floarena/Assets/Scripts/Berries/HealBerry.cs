using UnityEngine;
using Mirror;
public class HealBerry : MonoBehaviour {
  private MapVisualizer mapVisualizer;
  public float healMagnitude = BerryConstants.HEAL_BERRY_MAGNITUDE;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
  }
  private void OnTriggerEnter(Collider collider) 
  {
    if (collider.gameObject.GetComponent<MultiplayerThirdPersonController>().isLocalPlayer) {
      collider.gameObject.GetComponent<Health>().TakeHealing(healMagnitude);
    }
    Vector3 positionOnGrid = transform.position;
    mapVisualizer.SpawnPickupItem(positionOnGrid); // Trigger respawn timer
    Destroy(gameObject); // Destroy HealthConsumable
  }
}