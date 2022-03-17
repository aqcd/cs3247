using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public static MapGenerator instance;
    public GridVisualizer gridVisualizer;
    public MapVisualizer mapVisualizer;

    [Range(1, 10)] // Range of number of items on map
    public int numOfPickupItems;

    [Range(10, 20)] // Range of number of grass patches
    public int numOfBrush;

    [Range(40, 60)] // Range of map width and length
    public int width, length = 20;
    private MapGrid grid;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void GenerateMap(int seed) {
        Debug.Log("Generating map with seed: " + seed);
        Random.InitState(seed);
        grid = new MapGrid(width, length);
        gridVisualizer.VisualizeGrid(width, length);
        CandidateMap map = new CandidateMap(grid, numOfPickupItems, numOfBrush);
        map.CreateMap();
        mapVisualizer.VisualizeMap(grid, map.ReturnMapData());
    }
}
