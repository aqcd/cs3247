using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrush {
    public static List<Vector3> possiblePositions = new List<Vector3> {
        new Vector3(-1, 0, 0),
        new Vector3(-1, 0, 1),
        new Vector3( 0, 0, 1),
        new Vector3( 1, 0, 1),
        new Vector3( 1, 0, 0),
        new Vector3( 1, 0,-1),
        new Vector3( 0, 0,-1),
        new Vector3(-1, 0,-1)
    };

    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }

    public RandomBrush(Vector3 position) {
        this.Position = position;
    }
}
