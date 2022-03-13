using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedStructure {
    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }

    public FixedStructure(Vector3 position) {
        this.Position = position;
    }
}
