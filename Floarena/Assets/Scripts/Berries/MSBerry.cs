using UnityEngine;
using Mirror;
public class MSBerry : MonoBehaviour, IBerry {
  private MapVisualizer mapVisualizer;
  private float buffMagnitude = BerryConstants.MS_BERRY_MAGNITUDE;
  private float buffDuration = BerryConstants.MS_BERRY_DURATION;

  private Effect buffEffect;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
    buffEffect = new Effect(Attribute.MS, buffMagnitude);
  }

  public void Consume(PlayerManager playerManager)
  {
    Debug.Log(playerManager.gameObject);
    playerManager.BuffForDuration(buffEffect, buffDuration);
    Vector3 positionOnGrid = transform.position;
    mapVisualizer.SpawnPickupItem(positionOnGrid);
    Destroy(gameObject);
  }
}