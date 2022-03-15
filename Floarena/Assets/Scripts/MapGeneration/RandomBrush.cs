using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrush {
    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }

    public RandomBrush(Vector3 position) {
        this.Position = position;
    }
}
