using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapVisualizer : MonoBehaviour {
    private Transform parent;
    private MapGrid grid;
    private MapData data;
    public List<GameObject> liGoSpawn = new List<GameObject>(); // Prefabs for pickup items
    public GameObject wallPrefab; // Prefab for wall structure
    public GameObject brushPrefab; // Prefab for brush
    public GameObject rockPrefab; // Prefab for rock

    private IEnumerator coroutine;

    private void Awake() {
        parent = this.transform;
    }

    public void VisualizeMap(MapGrid grid, MapData data) {
        this.grid = grid;
        this.data = data;
        PlaceFixedStructures(grid, data);
        PlaceBackground();

        for (int i = 0; i < data.mapItemsArray.Length; i++) {
            if (data.mapItemsArray[i]) {
                var positionOnGrid = grid.CalculateCoordinatesFromIndex(i);

                if (PlacePickupItem(data, positionOnGrid)) { // Place pickup items
                    continue;
                }

                if (PlaceBrush(data, positionOnGrid)) {
                    continue;
                }

                if (PlaceRock(data, positionOnGrid)) {
                    continue;
                }
            }            
        }
    }

    private bool PlaceRock(MapData data, Vector3 positionOnGrid) {
        foreach (var rock in data.rocksList) {
            if (rock.Position == positionOnGrid) {
                float randomScale = Random.Range(0.0f, 2.0f);
                rockPrefab.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
                int randomRotation = Random.Range(0, 180);
                Quaternion rotation = Quaternion.Euler(0, randomRotation, 0);
                GameObject obj = Instantiate(rockPrefab, positionOnGrid, rotation);
                obj.transform.parent = gameObject.transform;
                rockPrefab.transform.localScale = Vector3.one;
                return true;
            }
        }
        return false;
    }

    private bool PlacePickupItem(MapData data, Vector3 positionOnGrid) {
        foreach (var pickupItem in data.pickupItemsList) {
            if (pickupItem.Position == positionOnGrid) {
                GameObject goToSpawn = liGoSpawn[Random.Range(0, liGoSpawn.Count)];
                GameObject obj = Instantiate(goToSpawn, positionOnGrid, Quaternion.identity);
                obj.transform.parent = gameObject.transform;
                return true;
            }
        }
        return false;
    }

    // Return a random valid coordinate to spawn berry
    public Vector3 GetRandomCoordinates() {
        var itemPlacementTryLimit = data.mapItemsArray.Length;
        while (itemPlacementTryLimit > 0) {
            var randomIndex = Random.Range(0, data.mapItemsArray.Length);
            if (data.mapItemsArray[randomIndex] == false) {
                data.mapItemsArray[randomIndex] = true;
                Vector3 coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                return coordinates;
            }
            itemPlacementTryLimit--;
        }
        return new Vector3(-1, 0, -1);
    }

    public void SetPositionAsEmpty(Vector3 position) {
        int indexAtPosition = grid.CalculateIndexFromCoordinates(position.x, position.z);
        data.mapItemsArray[indexAtPosition] = false;
    }

    private IEnumerator PlacePickupItemWithDelay(float waitTime, Vector3 positionOnGrid) {
        yield return new WaitForSeconds(waitTime);
        GameObject goToSpawn = liGoSpawn[Random.Range(0, liGoSpawn.Count)];
        GameObject obj = Instantiate(goToSpawn, positionOnGrid, Quaternion.identity);
        obj.transform.parent = gameObject.transform;
    }

    public void SpawnPickupItem(Vector3 positionOnGrid) {
        Vector3 spawnPosition = GetRandomCoordinates();
        SetPositionAsEmpty(positionOnGrid);
        coroutine = PlacePickupItemWithDelay(5.0f, spawnPosition);
        StartCoroutine(coroutine);
    }

    private bool PlaceBrush(MapData data, Vector3 positionOnGrid) {
        foreach (var brushItem in data.brushList) {
            if (brushItem.Position == positionOnGrid) {
                float randomHeight = Random.Range(0.0f, 0.3f);
                brushPrefab.transform.localScale += new Vector3(0.0f, randomHeight, 0.0f);
                int randomRotation = Random.Range(0, 180);
                Quaternion rotation = Quaternion.Euler(0, randomRotation, 0);

                GameObject obj = Instantiate(brushPrefab, positionOnGrid, rotation);
                obj.transform.parent = gameObject.transform;
                brushPrefab.transform.localScale = new Vector3(0.8f, 0.7f, 0.8f);
                return true;
            }
        }
        return false;
    }

    private void PlaceFixedStructures(MapGrid grid, MapData data) {
        foreach (var fixedStructure in data.fixedStructuresList) {
            var position = fixedStructure.Position;
            float randomHeight = Random.Range(0.0f, 0.8f);
            wallPrefab.transform.localScale += new Vector3(0.0f, randomHeight, 0.0f);
            GameObject obj = Instantiate(wallPrefab, position, Quaternion.identity);
            obj.transform.parent = gameObject.transform;
            wallPrefab.transform.localScale = Vector3.one;
        }
    }

    void PlaceBackground() {
        for (int i = -20; i < 80; i++) {
            for (int j = -1; j > -21; j--) {
                PlaceRandomItem(new Vector3(i, 0, j));
            }
        }

        for (int i = -20; i < 80; i++) {
            for (int j = 61; j < 80; j++) {
                PlaceRandomItem(new Vector3(i, 0, j));
            }
        }

        for (int i = -20; i < 0; i++) {
            for (int j = 0; j < 60; j++) {
                PlaceRandomItem(new Vector3(i, 0, j));
            }
        }

        for (int i = 61; i < 80; i++) {
            for (int j = 0; j < 60; j++) {
                PlaceRandomItem(new Vector3(i, 0, j));
            }
        }
    }

    void PlaceRandomItem(Vector3 position) {
        var prob = Random.Range(0, 4); // [0, 3)
        var prob2 = Random.Range(0, 10);
        if (prob == 0) { // Tree
            float randomHeight = Random.Range(0.0f, 0.8f);
            wallPrefab.transform.localScale += new Vector3(0.0f, randomHeight, 0.0f);
            GameObject obj = Instantiate(wallPrefab, position, Quaternion.identity);
            obj.transform.parent = gameObject.transform;
            wallPrefab.transform.localScale = Vector3.one;
        } else if (prob == 1 && prob2 == 2) { // Rock
            float randomScale = Random.Range(0.0f, 2.0f);
            rockPrefab.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
            int randomRotation = Random.Range(0, 180);
            Quaternion rotation = Quaternion.Euler(0, randomRotation, 0);
            GameObject obj = Instantiate(rockPrefab, position, rotation);
            obj.transform.parent = gameObject.transform;
            rockPrefab.transform.localScale = Vector3.one;
        } 
    }
}