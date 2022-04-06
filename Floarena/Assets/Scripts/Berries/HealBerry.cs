using UnityEngine;
public class HealBerry : MonoBehaviour, IBerry {
  private MapVisualizer mapVisualizer;
  public float healMagnitude = BerryConstants.HEAL_BERRY_MAGNITUDE;
  [SerializeField]
  private Canvas canvas;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
  }

  public void Consume(PlayerManager playerManager)
  {
    playerManager.gameObject.GetComponent<Health>().TakeHealing(healMagnitude);
    DestroySelf();
  }

  public void DestroySelf()
  {
    Vector3 positionOnGrid = transform.position;
    mapVisualizer.SpawnPickupItem(positionOnGrid);
    Destroy(gameObject);
  }
  public void EnableCanvas()
  {
    canvas.enabled = true;
  }

  public void DisableCanvas()
  {
    canvas.enabled = false;
  }
}