using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
    private int x, z; // Store position
    private bool isTaken;

    public int X { get => x; }
    public int Z { get => z; }
    public bool IsTaken { get => isTaken; set => isTaken = value; }

    public Cell(int x, int z) {
        this.x = x;
        this.z = z;
        isTaken = false;
    }
}
