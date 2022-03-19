using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthItem : NetworkBehaviour {
    public List<GameObject> liGoSpawn = new List<GameObject>(); // Prefabs for pickup items

    private IEnumerator coroutine;

    private IEnumerator PlacePickupItemWithDelay(float waitTime, Vector3 positionOnGrid) {
        yield return new WaitForSeconds(waitTime);
        GameObject goToSpawn = liGoSpawn[Random.Range(0, liGoSpawn.Count)];
        Instantiate(goToSpawn, positionOnGrid, Quaternion.identity);
    }

    public void SpawnPickupItem(Vector3 positionOnGrid) {
        coroutine = PlacePickupItemWithDelay(5.0f, positionOnGrid);
        StartCoroutine(coroutine);
    }

}
