using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapVisualizer : MonoBehaviour {
    private Transform parent;
    public List<GameObject> liGoSpawn = new List<GameObject>(); // Prefabs for pickup items
    public List<GameObject> berryPrefabs = new List<GameObject>(); // Prefabs for berries
    public GameObject wallPrefab; // Prefab for wall structure
    public GameObject brushPrefab; // Prefab for brush
    public GameObject rockPrefab; 

    private IEnumerator coroutine;

    private void Awake() {
        parent = this.transform;
    }

    public void VisualizeMap(MapGrid grid, MapData data) {
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

                if (PlaceBerry(data, positionOnGrid)) {
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
                GameObject goToSpawn = liGoSpawn[0]; // index 0 for health consumable
                Instantiate(goToSpawn, positionOnGrid + offset, Quaternion.identity);
                return true;
            }
        }
        return false;
    }

    private bool PlaceBerry(MapData data, Vector3 positionOnGrid) {
        foreach (var berry in data.berryList) {
            if (berry.Position == positionOnGrid) {
                Vector3 offset = new Vector3(0f, -1.0f, 0f);
                GameObject goToSpawn = berryPrefabs[Random.Range(0, berryPrefabs.Count)];
                Instantiate(goToSpawn, positionOnGrid + offset, Quaternion.identity);
                return true;
            }
        }
        return false;
    }

    private IEnumerator PlacePickupItemWithDelay(float waitTime, int arrIndex, Vector3 positionOnGrid) {
        yield return new WaitForSeconds(waitTime);
        GameObject goToSpawn = liGoSpawn[arrIndex];
        Instantiate(goToSpawn, positionOnGrid, Quaternion.identity);
    }

    public void SpawnPickupItem(int arrIndex, Vector3 positionOnGrid) {
        coroutine = PlacePickupItemWithDelay(5.0f, arrIndex, positionOnGrid);
        StartCoroutine(coroutine);
    }

    private IEnumerator PlaceBerryWithDelay(float waitTime, Vector3 positionOnGrid) {
        yield return new WaitForSeconds(waitTime);
        GameObject goToSpawn = berryPrefabs[Random.Range(0, berryPrefabs.Count)];
        Instantiate(goToSpawn, positionOnGrid, Quaternion.identity);
    }

    public void SpawnBerry(Vector3 positionOnGrid) {
        coroutine = PlaceBerryWithDelay(5.0f, positionOnGrid);
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
