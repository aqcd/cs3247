using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandidateMap {
    // Place items on map
    private MapGrid grid;
    private int numberOfPickupItems = 0;
    private bool[] pickupItemsArray = null;
    private bool[] fixedStructuresArray = null;
    private List<PickupItem> pickupItemsList;
    private List<FixedStructure> fixedStructuresList;

    public MapGrid Grid { get => grid; }
    public bool[] PickupItemsArray { get => pickupItemsArray; }
    public bool[] FixedStructuresArray { get => fixedStructuresArray; }

    public CandidateMap(MapGrid grid, int numberOfPickupItems) {
        this.numberOfPickupItems = numberOfPickupItems;
        this.grid = grid;
    }

    public void CreateMap(bool autoRepair = false) {
        pickupItemsArray = new bool[grid.Width * grid.Length];
        fixedStructuresArray = new bool[grid.Width * grid.Length];
        this.pickupItemsList = new List<PickupItem>();
        this.fixedStructuresList = new List<FixedStructure>();
        InitializeFixedStructures();
        RandomlyPlacePickupItems(this.numberOfPickupItems);
    }

    private bool CheckIfPositionCanBeObstacle(Vector3 position) {
        int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

        return (pickupItemsArray[index] == false) && (fixedStructuresArray[index] == false);
    }

    private void RandomlyPlacePickupItems(int numberOfPickupItems) {
        var count = numberOfPickupItems;
        var itemPlacementTryLimit = 100;
        while (count > 0 && itemPlacementTryLimit > 0) {
            var randomIndex = Random.Range(0, pickupItemsArray.Length);
            if (pickupItemsArray[randomIndex] == false && fixedStructuresArray[randomIndex] == false) {
                // Free space
                var coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                pickupItemsArray[randomIndex] = true;
                pickupItemsList.Add(new PickupItem(coordinates)); // Placed item
                count--;
            }
            itemPlacementTryLimit--;
        }
    }

    private void PlaceWallsForStructure(FixedStructure fixedStructure, int typeOfWall) {
        List<Vector3> wallOffset = new List<Vector3> { };
        if (typeOfWall == 0) { // diagonal
            wallOffset = FixedStructure.listOfWalls;
        } else if (typeOfWall == 1) { // L
            wallOffset = FixedStructure.listOfWalls1;
        } else if (typeOfWall == 2) { // other L
            wallOffset = FixedStructure.listOfWalls2;
        } 

        foreach (var position in wallOffset) {
            var newPosition = fixedStructure.Position + position; // position is an offset 
            if (grid.IsCellValid(newPosition.x, newPosition.z) && CheckIfPositionCanBeObstacle(newPosition)) {
                int index = grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.z);
                var coordinates = grid.CalculateCoordinatesFromIndex(index);
                fixedStructuresArray[index] = true;
                fixedStructuresList.Add(new FixedStructure(coordinates));
            }
        }
    }

    private void InitializeFixedStructures() {     
        int diag1 = grid.Length * (int)(0.75 * grid.Width) + (int)(0.25 * grid.Length);
        int diag2 = grid.Length * (int)(0.25 * grid.Width) + (int)(0.75 * grid.Length);
        int L1 = grid.Length * (int)(0.20 * grid.Width) + 3;
        int L2 = grid.Length * (int)(0.80 * grid.Width) + (grid.Length - 3);

        int[] indexArr = { diag1, diag2 };
        int[] straightWallArr = { L1, L2 };

        for (int i = 0; i < indexArr.Length; i++) {
            var coordinates = grid.CalculateCoordinatesFromIndex(indexArr[i]);
            pickupItemsArray[indexArr[i]] = true; // So that pickup items do not spawn in walls
            fixedStructuresArray[indexArr[i]] = true;
            fixedStructuresList.Add(new FixedStructure(coordinates));
        }

        for (int m = 0; m < straightWallArr.Length; m++) {
            var coordinates = grid.CalculateCoordinatesFromIndex(straightWallArr[m]);
            pickupItemsArray[straightWallArr[m]] = true; 
            fixedStructuresArray[straightWallArr[m]] = true;
            fixedStructuresList.Add(new FixedStructure(coordinates));
        }

        PlaceWallsForStructure(fixedStructuresList[0], 0); // diagonal wall
        PlaceWallsForStructure(fixedStructuresList[1], 0);
        PlaceWallsForStructure(fixedStructuresList[2], 1); // L
        PlaceWallsForStructure(fixedStructuresList[3], 2); // other L
    }

    public MapData ReturnMapData() {
        return new MapData {
            pickupItemsArray = this.pickupItemsArray,
            fixedStructuresArray = this.fixedStructuresArray,
            pickupItemsList = pickupItemsList,
            fixedStructuresList = fixedStructuresList
        };
    }
}
