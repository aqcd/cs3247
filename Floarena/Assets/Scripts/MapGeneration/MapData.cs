using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapData {
    public bool[] pickupItemsArray;
    public bool[] fixedStructuresArray;
    public List<PickupItem> pickupItemsList;
    public List<FixedStructure> fixedStructuresList;
}