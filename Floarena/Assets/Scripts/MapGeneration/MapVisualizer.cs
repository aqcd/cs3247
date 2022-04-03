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

        for (int i = 0; i < data.mapItemsArray.Length; i++) {
            if (data.mapItemsArray[i]) {
                var positionOnGrid = grid.CalculateCoordinatesFromIndex(i);
                grid.SetCell(positionOnGrid.x, positionOnGrid.z, CellObjectType.PickupItem);

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
                Vector3 offset = new Vector3(0f, -1.0f, 0f);
                int randomRotation = Random.Range(0, 180);
                Quaternion rotation = Quaternion.Euler(0, randomRotation, 0);
                Instantiate(rockPrefab, positionOnGrid + offset, rotation);
                rockPrefab.transform.localScale = Vector3.one;
                return true;
            }
        }
        return false;
    }

    private bool PlacePickupItem(MapData data, Vector3 positionOnGrid) {
        foreach (var pickupItem in data.pickupItemsList) {
            if (pickupItem.Position == positionOnGrid) {
                Vector3 offset = new Vector3(0f, -1.0f, 0f);
                GameObject goToSpawn = liGoSpawn[Random.Range(0, liGoSpawn.Count)];
                Instantiate(goToSpawn, positionOnGrid + offset, Quaternion.identity);
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
                return new Vector3(coordinates.x, -1, coordinates.z);
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
        Instantiate(goToSpawn, positionOnGrid, Quaternion.identity);
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
                float randomHeight = Random.Range(0.0f, 0.8f);
                brushPrefab.transform.localScale += new Vector3(0.0f, randomHeight, 0.0f);
                Vector3 offset = new Vector3(0f, -1.0f, 0f);
                Instantiate(brushPrefab, positionOnGrid + offset, Quaternion.identity);
                brushPrefab.transform.localScale = Vector3.one;
                return true;
            }
        }
        return false;
    }

    private void PlaceFixedStructures(MapGrid grid, MapData data) {
        foreach (var fixedStructure in data.fixedStructuresList) {
            var obstaclePosition = fixedStructure.Position;
            grid.SetCell(obstaclePosition.x, obstaclePosition.z, CellObjectType.FixedStructure);
            Instantiate(wallPrefab, obstaclePosition, Quaternion.identity);
            //CreateIndicator(obstaclePosition, Color.red, PrimitiveType.Cube);
        }
    }

    // Spawn primitive indicator when prefabs are not used 
    private void CreateIndicator(Vector3 position, Color color, PrimitiveType sphere) {
        var element = GameObject.CreatePrimitive(sphere);
        element.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
        element.transform.parent = parent;
        var renderer = element.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
    }
}