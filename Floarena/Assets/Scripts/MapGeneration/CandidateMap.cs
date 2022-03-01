using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandidateMap {
    // Place obstacles on map
    private MapGrid grid;
    private int numberOfPickupItems = 0;
    private bool[] obstaclesArray = null;
    private List<PickupItem> pickupItemsList;
    private List<FixedStructure> fixedStructuresList;

    public MapGrid Grid { get => grid; }
    public bool[] ObstaclesArray { get => obstaclesArray; }

    public CandidateMap(MapGrid grid, int numberOfPickupItems) {
        this.numberOfPickupItems = numberOfPickupItems;
        this.grid = grid;
    }

    public void CreateMap(bool autoRepair = false) {
        obstaclesArray = new bool[grid.Width * grid.Length];
        this.pickupItemsList = new List<PickupItem>();
        this.fixedStructuresList = new List<FixedStructure>();
        InitializeFixedStructures();
        RandomlyPlacePickupItems(this.numberOfPickupItems);
    }

    private bool CheckIfPositionCanBeObstacle(Vector3 position) {
        int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

        return obstaclesArray[index] == false;
    }

    private void RandomlyPlacePickupItems(int numberOfPickupItems) {
        var count = numberOfPickupItems;
        var itemPlacementTryLimit = 100;
        while (count > 0 && itemPlacementTryLimit > 0) {
            var randomIndex = Random.Range(0, obstaclesArray.Length);
            if (obstaclesArray[randomIndex] == false) {
                // Free space
                var coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                obstaclesArray[randomIndex] = true;
                pickupItemsList.Add(new PickupItem(coordinates)); // Placed item
                count--;
            }
            itemPlacementTryLimit--;
        }
    }

    private void InitializeFixedStructures() {
        int index = 5; // Index for one fixed structure
        var coordinates = grid.CalculateCoordinatesFromIndex(index);
        obstaclesArray[index] = true;
        fixedStructuresList.Add(new FixedStructure(coordinates));
    }

    public MapData ReturnMapData() {
        return new MapData {
            obstacleArray = this.obstaclesArray,
            pickupItemsList = pickupItemsList,
            fixedStructuresList = fixedStructuresList
        };
    }
}
