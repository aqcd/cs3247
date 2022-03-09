using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedStructure {
    public static List<Vector3> listOfWalls = new List<Vector3> {
        new Vector3(-2, 0, 2),
        new Vector3(-1, 0, 2),
        new Vector3(-1, 0, 1),
        new Vector3( 0, 0, 1),
        new Vector3( 1, 0, 0),
        new Vector3( 1, 0,-1),
        new Vector3( 2, 0,-1),
        new Vector3( 2, 0,-2)
    };

    public static List<Vector3> listOfWalls1 = new List<Vector3> {
        new Vector3( 0, 0, 1),
        new Vector3( 0, 0, 2),
        new Vector3( 0, 0, 3),
        new Vector3( 0, 0, 4),
        new Vector3( 1, 0, 0),
        new Vector3( 2, 0, 0),
        new Vector3( 3, 0, 0),
        new Vector3( 4, 0, 0)
    };

    public static List<Vector3> listOfWalls2 = new List<Vector3> {
        new Vector3( 0, 0,-1),
        new Vector3( 0, 0,-2),
        new Vector3( 0, 0,-3),
        new Vector3( 0, 0,-4),
        new Vector3(-1, 0, 0),
        new Vector3(-2, 0, 0),
        new Vector3(-3, 0, 0),
        new Vector3(-4, 0, 0)
    };

    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }

    public FixedStructure(Vector3 position) {
        this.Position = position;
    }
}
