using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapData {
    public bool[] mapItemsArray;
    public List<PickupItem> pickupItemsList;
    public List<FixedStructure> fixedStructuresList;
    public List<RandomBrush> brushList;
    public List<Rock> rocksList;
    public List<DemoBerry> berryList;
}