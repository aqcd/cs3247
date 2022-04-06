using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ASBerry : NetworkBehaviour, IBerry
{
  private MapVisualizer mapVisualizer;
  private float buffMagnitude = BerryConstants.AS_BERRY_MAGNITUDE;
  private float buffDuration = BerryConstants.AS_BERRY_DURATION;
  [SerializeField]
  private Canvas canvas;
  private Effect buffEffect;
  void Start()
  {
    mapVisualizer = GameObject.FindWithTag("MapVisualizer").GetComponent<MapVisualizer>();
    buffEffect = new Effect(Attribute.AS, buffMagnitude);
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
    Destroy(gameObject.transform.parent.gameObject);
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
