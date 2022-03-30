using UnityEngine;
using Mirror;
public class ADBerry : MonoBehaviour {
  private MapVisualizer mapVisualizer;
  public int buffMagnitude = 3;
  public float buffDuration = 15;

  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
  }
  private void OnTriggerEnter(Collider collider) 
  {
    if (collider.gameObject.GetComponent<MultiplayerThirdPersonController>().isLocalPlayer) {
        collider.gameObject.GetComponent<PlayerManager>();
    }
    Vector3 positionOnGrid = transform.position;
    mapVisualizer.SpawnPickupItem(positionOnGrid);
    Destroy(gameObject);
  }
}