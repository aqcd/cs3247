using UnityEngine;
public class ADBerry : MonoBehaviour, IBerry {
  private MapVisualizer mapVisualizer;
  private float buffMagnitude = BerryConstants.AD_BERRY_MAGNITUDE;
  private float buffDuration = BerryConstants.AD_BERRY_DURATION;
  [SerializeField]
  private Canvas canvas;
  private Effect buffEffect;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
    buffEffect = new Effect(Attribute.AD, buffMagnitude);
  }

  public void Consume(PlayerManager playerManager)
  {
    playerManager.BuffForDuration(buffEffect, buffDuration);
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