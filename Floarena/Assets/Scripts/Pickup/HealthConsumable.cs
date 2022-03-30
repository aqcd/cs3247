using UnityEngine;
using Mirror;
public class HealthConsumable : MonoBehaviour {
  private MapVisualizer mapVisualizer;
  public int healMagnitude = 20;
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