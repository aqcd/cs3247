using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisualizer : MonoBehaviour {
    private Transform parent;
    public List<GameObject> liGoSpawn = new List<GameObject>(); // Prefabs for pickup items

    private void Awake() {
        parent = this.transform;
    }

    public void VisualizeMap(MapGrid grid, MapData data, bool visualizeUsingPrefabs) {
        if (visualizeUsingPrefabs) {
            // To implement
        } else {
            VisualizeUsingPrimitives(grid, data);
        }
    }
    
    private void VisualizeUsingPrimitives(MapGrid grid, MapData data) {
        for (int i = 0; i < data.obstacleArray.Length; i++) {
            if (data.obstacleArray[i]) {
                var positionOnGrid = grid.CalculateCoordinatesFromIndex(i);
                grid.SetCell(positionOnGrid.x, positionOnGrid.z, CellObjectType.PickupItem);

                if (PlacePickupItem(data, positionOnGrid)) { // Place pickup items
                    continue;
                }
            }
        }

        PlaceFixedStructures(grid, data);
    }

    private bool PlacePickupItem(MapData data, Vector3 positionOnGrid) {
        foreach (var pickupItem in data.pickupItemsList) {
            if (pickupItem.Position == positionOnGrid) {
                //CreateIndicator(positionOnGrid, Color.red, PrimitiveType.Cube);
                Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);
                GameObject goToSpawn = liGoSpawn[Random.Range(0, liGoSpawn.Count)];
                Instantiate(goToSpawn, positionOnGrid + offset, Quaternion.identity);
                return true;
            }
        }
        return false;
    }

    private void PlaceFixedStructures(MapGrid grid, MapData data) {
        foreach (var fixedStructure in data.fixedStructuresList) {
            var obstaclePosition = fixedStructure.Position;
            grid.SetCell(obstaclePosition.x, obstaclePosition.z, CellObjectType.FixedStructure);
            CreateIndicator(obstaclePosition, Color.red, PrimitiveType.Cube); // Use prefabs later
        }
    }

    // Spawn primitive indicator when prefabs are not used 
    private void CreateIndicator(Vector3 position, Color color, PrimitiveType sphere) {
        var element = GameObject.CreatePrimitive(sphere);
        element.transform.position = position + new Vector3(.5f, .5f, .5f);
        element.transform.parent = parent;
        var renderer = element.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
    }
}
