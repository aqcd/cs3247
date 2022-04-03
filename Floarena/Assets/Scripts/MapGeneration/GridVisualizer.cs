using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour {
    public GameObject groundPrefab;

    public void VisualizeGrid(int width, int length) {
        Vector3 position = new Vector3(width / 2f - 0.5f, 0f, length / 2f - 0.5f);
        Quaternion rotation = Quaternion.Euler(90, 0, 0);
        GameObject board = Instantiate(groundPrefab, position, rotation);
        board.transform.localScale = new Vector3(width, length, 1);
        board.transform.parent = gameObject.transform;

        // Surrounding quads for background 
        Vector3 bgPosition1 = new Vector3(width / 2f - 0.5f, 0f, length / 2f - 0.5f + 40.0f);
        GameObject background1 = Instantiate(groundPrefab, bgPosition1, rotation);
        background1.transform.localScale = new Vector3(60, 20, 1);
        background1.transform.parent = gameObject.transform;

        Vector3 bgPosition2 = new Vector3(width / 2f - 0.5f, 0f, length / 2f - 0.5f - 40.0f);
        GameObject background2 = Instantiate(groundPrefab, bgPosition2, rotation);
        background2.transform.localScale = new Vector3(60, 20, 1);
        background2.transform.parent = gameObject.transform;

        Vector3 bgPosition3 = new Vector3(width / 2f - 0.5f - 40.0f, 0f, length / 2f - 0.5f);
        GameObject background3 = Instantiate(groundPrefab, bgPosition3, rotation);
        background3.transform.localScale = new Vector3(20, 100, 1);
        background3.transform.parent = gameObject.transform;

        Vector3 bgPosition4 = new Vector3(width / 2f - 0.5f + 40.0f, 0f, length / 2f - 0.5f);
        GameObject background4 = Instantiate(groundPrefab, bgPosition4, rotation);
        background4.transform.localScale = new Vector3(20, 100, 1);
        background4.transform.parent = gameObject.transform;
    }
}