using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchHandler : MonoBehaviour {
    public static UISwitchHandler instance;
    
    public GameObject homeScreen;
    public GameObject loadoutScreen;
    public GameObject waitScreen;

    // This gets triggered once more time when the server makes a switch to the "online" scene
    // which is a duplicate of the "offline" scene
    void Start() {
        if (instance == null) {
            instance = this;
        }

        if (GameManager.instance.isNetworkActive) {
            // If we are in the online scene now, display the waiting for players prompt
            DisplayWaitScreen();
        } else {
            DisplayHomeScreen();
        }
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
