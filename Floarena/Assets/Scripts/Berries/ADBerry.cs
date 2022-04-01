using UnityEngine;
using Mirror;
public class ADBerry : MonoBehaviour, IBerry {
  private MapVisualizer mapVisualizer;
  private float buffMagnitude = BerryConstants.AD_BERRY_MAGNITUDE;
  private float buffDuration = BerryConstants.AD_BERRY_DURATION;

  private Effect buffEffect;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
    buffEffect = new Effect(Attribute.AD, buffMagnitude);
  }

  public void Consume(PlayerManager playerManager)
  {
    playerManager.BuffForDuration(buffEffect, buffDuration);
    Vector3 positionOnGrid = transform.position;
    mapVisualizer.SpawnPickupItem(positionOnGrid);
    Destroy(gameObject);
  }
}