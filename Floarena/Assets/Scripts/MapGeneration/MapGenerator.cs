using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GridVisualizer gridVisualizer;
    public MapVisualizer mapVisualizer;

    public bool randomPlacement;
    [Range(1, 10)] // Range of number of items on map
    public int numOfPickupItems;

    [Range(3, 20)] // Range of map width and length
    public int width, length = 11;
    private MapGrid grid;

    void Start() {
        grid = new MapGrid(width, length);
        gridVisualizer.VisualizeGrid(width, length);
        CandidateMap map = new CandidateMap(grid, numOfPickupItems);
        map.CreateMap();
        mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);
    }
}
