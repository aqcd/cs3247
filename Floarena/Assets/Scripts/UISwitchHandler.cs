using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchHandler : MonoBehaviour {
    public GameObject homeScreen;
    public GameObject loadoutScreen;

    void Start() {
        DisplayHomeScreen();
    }

    public void DisplayHomeScreen() {
        loadoutScreen.SetActive(false);
        homeScreen.SetActive(true);
    }
    
    public void DisplayLoadoutScreen() {
        loadoutScreen.SetActive(true);
        homeScreen.SetActive(false);
    }
}
