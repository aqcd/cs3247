using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASBerry : MonoBehaviour, IBerry
{
    private MapVisualizer mapVisualizer;
  private float buffMagnitude = BerryConstants.AS_BERRY_MAGNITUDE;
  private float buffDuration = BerryConstants.AS_BERRY_DURATION;

  private Effect buffEffect;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
    buffEffect = new Effect(Attribute.AS, buffMagnitude);
  }

  public void Consume(PlayerManager playerManager)
  {
    playerManager.BuffForDuration(buffEffect, buffDuration);
    Vector3 positionOnGrid = transform.position;
    mapVisualizer.SpawnPickupItem(positionOnGrid);
    Destroy(gameObject);
  }
}
