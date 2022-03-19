using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandidateMap {
    // Place items on map
    private MapGrid grid;
    private int numberOfPickupItems = 0;
    private int numberOfBrush = 0;
    private bool[] mapItemsArray = null;
    private List<PickupItem> pickupItemsList;
    private List<FixedStructure> fixedStructuresList;
    private List<RandomBrush> brushList;

    public MapGrid Grid { get => grid; }
    public bool[] MapItemsArray { get => mapItemsArray; }

    public CandidateMap(MapGrid grid, int numberOfPickupItems, int numberOfBrush) {
        this.numberOfPickupItems = numberOfPickupItems;
        this.numberOfBrush = numberOfBrush;
        this.grid = grid;
    }

    public void CreateMap(bool autoRepair = false) {
        mapItemsArray = new bool[grid.Width * grid.Length];
        this.pickupItemsList = new List<PickupItem>();
        this.fixedStructuresList = new List<FixedStructure>();
        this.brushList = new List<RandomBrush>();
        InitializeFixedStructures();
        RandomlyPlacePickupItems(this.numberOfPickupItems);
        RandomlyPlaceBrush(this.numberOfBrush);
    }

    private bool CheckIfPositionCanBeObstacle(Vector3 position) {
        int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

        return (mapItemsArray[index] == false);
    }

    private void RandomlyPlacePickupItems(int numberOfPickupItems) {
        var count = numberOfPickupItems;
        var itemPlacementTryLimit = 100;
        while (count > 0 && itemPlacementTryLimit > 0) {
            var randomIndex = Random.Range(0, mapItemsArray.Length);
            if (mapItemsArray[randomIndex] == false) {
                // Free space
                var coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                mapItemsArray[randomIndex] = true;
                pickupItemsList.Add(new PickupItem(coordinates)); // Placed item
                count--;
            }
            itemPlacementTryLimit--;
        }
    }

    private void RandomlyPlaceBrush(int numberOfBrush) {
        var count = numberOfBrush;
        var itemPlacementTryLimit = 100;
        while (count > 0 && itemPlacementTryLimit > 0) {
            var randomIndex = Random.Range(0, mapItemsArray.Length);
            var randomHeight = Random.Range(-0.2f, 0.2f);
            if (mapItemsArray[randomIndex] == false) {
                // Free space
                var coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                mapItemsArray[randomIndex] = true;
                brushList.Add(new RandomBrush(coordinates)); // Placed item
                PlaceNeighboringBrush(coordinates + new Vector3(0, 0, randomHeight), randomIndex);
                count--;
            }
            itemPlacementTryLimit--;
        }
    }

    private void PlaceNeighboringBrush(Vector3 coordinates, int randomIndex) {
        int brushSize = 5; // 5x5 grid 
        for (int i = 0; i < brushSize; i++) {
            for (int j = 0; j < brushSize; j++) {
                AddBrush(coordinates + new Vector3(i, 0, j));
            }
        }
    }

    private void AddBrush(Vector3 newPosition) {
        var randomProb = Random.Range(0, 5);
        if (randomProb < 3) {
            if (grid.IsCellValid(newPosition.x, newPosition.z) && CheckIfPositionCanBeObstacle(newPosition)) {
                int index = grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.z);
                var coordinates = grid.CalculateCoordinatesFromIndex(index);
                mapItemsArray[index] = true;
                brushList.Add(new RandomBrush(coordinates));
            }
        }
    }
    
    private void PlaceMapBounds(Vector3 position) {
        for (int i = 0; i < grid.Width; i++) {
            AddWall(position + new Vector3(i, 0, 0));
            AddWall(position + new Vector3(i, 0, grid.Length - 1));
        }
        for (int j = 0; j < grid.Length; j++) {
            AddWall(position + new Vector3(0, 0, j));
            AddWall(position + new Vector3(grid.Width - 1, 0, j));
        }
    }

    private void PlaceWallsForStructure(FixedStructure fixedStructure, int typeOfWall) {
        Vector3 position;
        if (typeOfWall == 0) { // diagonal
            int halfLength = (int)(0.5 * grid.Length);
            position = fixedStructure.Position;
            for (int i = 0; i < halfLength; i++) {
                AddWall(position + new Vector3(1, 0, 0)); // right
                AddWall(position + new Vector3(2, 0, 0)); // right
                position = position + new Vector3(1, 0, 0);
                AddWall(position + new Vector3(0, 0, -1)); // down
                position = position + new Vector3(0, 0, -1);
            }
        } else if (typeOfWall == 1) { // L
            int thirdLength = (int)(0.3 * grid.Length);
            position = fixedStructure.Position;
            for (int i = 0; i < thirdLength; i++) {
                AddWall(position + new Vector3(1, 0, 0)); // right
                AddWall(position + new Vector3(1, 0, 1)); // right & up
                position = position + new Vector3(1, 0, 0);
            }
            position = fixedStructure.Position;
            for (int j = 0; j < thirdLength; j++) {
                AddWall(position + new Vector3(0, 0, 1)); // up
                AddWall(position + new Vector3(1, 0, 1)); // up & right
                position = position + new Vector3(0, 0, 1);
            }
        } else if (typeOfWall == 2) { // other L
            int thirdLength = (int)(0.3 * grid.Length);
            position = fixedStructure.Position;
            for (int i = 0; i < thirdLength; i++) {
                AddWall(position + new Vector3(-1, 0, 0)); // left
                AddWall(position + new Vector3(-1, 0, 1)); // left & up
                position = position + new Vector3(-1, 0, 0);
            }
            position = fixedStructure.Position;
            for (int j = 0; j < thirdLength; j++) {
                AddWall(position + new Vector3(0, 0, -1)); // down
                AddWall(position + new Vector3(-1, 0, -1)); // down & left
                position = position + new Vector3(0, 0, -1);
            }
        }

        position = grid.CalculateCoordinatesFromIndex(0);
        PlaceMapBounds(position);
    }

    private void AddWall(Vector3 newPosition) {
        if (grid.IsCellValid(newPosition.x, newPosition.z) && CheckIfPositionCanBeObstacle(newPosition)) {
            int index = grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.z);
            var coordinates = grid.CalculateCoordinatesFromIndex(index);
            mapItemsArray[index] = true;
            fixedStructuresList.Add(new FixedStructure(coordinates));
        }
    }

    private void InitializeFixedStructures() {     
        int diag1 = grid.Length * (int)(0.75 * grid.Width) + (int)(0.25 * grid.Length);
        int L1 = grid.Length * (int)(0.20 * grid.Width) + (int)(0.20 * grid.Length);
        int L2 = grid.Length * (int)(0.80 * grid.Width) + (grid.Length - (int)(0.20 * grid.Length));

        int[] indexArr = { diag1 };
        int[] straightWallArr = { L1, L2 };

        for (int i = 0; i < indexArr.Length; i++) {
            var coordinates = grid.CalculateCoordinatesFromIndex(indexArr[i]);
            mapItemsArray[indexArr[i]] = true; 
            fixedStructuresList.Add(new FixedStructure(coordinates));
        }

        for (int m = 0; m < straightWallArr.Length; m++) {
            var coordinates = grid.CalculateCoordinatesFromIndex(straightWallArr[m]);
            mapItemsArray[straightWallArr[m]] = true; 
            fixedStructuresList.Add(new FixedStructure(coordinates));
        }

        PlaceWallsForStructure(fixedStructuresList[0], 0); // diagonal wall
        PlaceWallsForStructure(fixedStructuresList[1], 1); // L
        PlaceWallsForStructure(fixedStructuresList[2], 2); // other L
    }

    public MapData ReturnMapData() {
        return new MapData {
            mapItemsArray = this.mapItemsArray,
            pickupItemsList = pickupItemsList,
            fixedStructuresList = fixedStructuresList,
            brushList = brushList
        };
    }
}
