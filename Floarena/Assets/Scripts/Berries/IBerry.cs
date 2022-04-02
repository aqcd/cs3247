using System.Collections;
using UnityEngine;

public interface IBerry
{
    public void Consume(PlayerManager playerManager);
    public void DestroySelf();
}
