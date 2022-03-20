using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchHandler : MonoBehaviour {
    public static UISwitchHandler instance;
    
    public GameObject homeScreen;
    public GameObject loadoutScreen;
    public GameObject waitScreen;

    void Start() {
        if (instance == null) {
            instance = this;
        }
        DisplayHomeScreen();
    }

    public void DisplayHomeScreen() {
        waitScreen.SetActive(false);
        loadoutScreen.SetActive(false);
        homeScreen.SetActive(true);
    }
    
    public void DisplayLoadoutScreen() {
        waitScreen.SetActive(false);
        loadoutScreen.SetActive(true);
        homeScreen.SetActive(false);
    }

    public void DisplayWaitScreen() {
        waitScreen.SetActive(true);
        loadoutScreen.SetActive(false);
        homeScreen.SetActive(false);
    }
}
