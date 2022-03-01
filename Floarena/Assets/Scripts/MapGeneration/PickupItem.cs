using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem {
    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }

    public PickupItem(Vector3 position) {
        this.Position = position;
    }
}
