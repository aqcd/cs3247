using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GridVisualizer gridVisualizer;
    public MapVisualizer mapVisualizer;

    [Range(1, 10)] // Range of number of items on map
    public int numOfPickupItems;

    [Range(10, 20)] // Range of number of grass patches
    public int numOfBrush;

    [Range(40, 60)] // Range of map width and length
    public int width, length = 20;
    private MapGrid grid;

    void Start() {
        // TODO: use room code id as seed 
        const int initialSeed = 12; 
        Random.InitState(initialSeed);
        grid = new MapGrid(width, length);
        gridVisualizer.VisualizeGrid(width, length);
        CandidateMap map = new CandidateMap(grid, numOfPickupItems, numOfBrush);
        map.CreateMap();
        mapVisualizer.VisualizeMap(grid, map.ReturnMapData());
    }
}
