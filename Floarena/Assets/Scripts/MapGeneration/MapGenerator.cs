using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour {
    public static MapGenerator instance;
    public GridVisualizer gridVisualizer;
    public MapVisualizer mapVisualizer;

    [Range(1, 10)] // Range of number of items on map
    public int numOfPickupItems;

    [Range(10, 20)] // Range of number of grass patches
    public int numOfBrush;

    [Range(10, 30)] // Range of number of grass patches
    public int numOfRocks;

    [Range(1, 3)] // Range of number of berry
    public int numOfBerry;

    [Range(40, 60)] // Range of map width and length
    public int width, length = 20;
    private MapGrid grid;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        // Generate map for debugging if we are not in the actual game scene
        if (SceneManager.GetActiveScene().name != "MapWithPlayer") {
            GenerateMap(10);
        }
    }

    public void GenerateMap(int seed) {
        Debug.Log("Generating map with seed: " + seed);
        Random.InitState(seed);
        grid = new MapGrid(width, length);
        gridVisualizer.VisualizeGrid(width, length);
        CandidateMap map = new CandidateMap(grid, numOfPickupItems, numOfBrush, numOfRocks, numOfBerry);
        map.CreateMap();
        mapVisualizer.VisualizeMap(grid, map.ReturnMapData());
    }
}
