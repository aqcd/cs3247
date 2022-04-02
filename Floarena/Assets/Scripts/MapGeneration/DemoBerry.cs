using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBerry {
    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }

    public DemoBerry(Vector3 position) {
        this.Position = position;
    }
}