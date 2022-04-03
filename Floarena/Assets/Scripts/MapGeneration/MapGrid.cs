using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid {
    private int width, length;
    private Cell[,] cellGrid;

    public int Width { get => width; }
    public int Length { get => length; }

    public MapGrid(int width, int length) {
        this.width = width;
        this.length = length;
        CreateGrid();
    }

    private void CreateGrid() {
        cellGrid = new Cell[length, width];
        for (int row = 0; row < length; row++) {
            for (int col = 0; col < width; col++) {
                cellGrid[row, col] = new Cell(col, row);
            }
        }
    }

    public bool IsCellTaken(int x, int z) {
        return cellGrid[z, x].IsTaken;
    }

    public bool IsCellTaken(float x, float z) {
        return cellGrid[(int)z, (int)x].IsTaken;
    }

    public bool IsCellValid(float x, float z) {
        if (x >= width || x < 0 || z >= length || z < 0) {
            return false;
        }
        return true;
    }

    public Cell GetCell(int x, int z) {
        if (IsCellValid(x, z) == false) {
            return null;
        }
        return cellGrid[z, x];
    }

    public Cell GetCell(float x, float z) {
        return GetCell((int)x, (int)z);
    }

    public int CalculateIndexFromCoordinates(int x, int z) {
        return x + z * width;
    }

    public int CalculateIndexFromCoordinates(float x, float z) {
        return (int)x + (int)z * width;
    }

    public Vector3 CalculateCoordinatesFromIndex(int randomIndex) {
        int x = randomIndex % width;
        int z = randomIndex / width;
        return new Vector3(x, 0, z);
    }
}
