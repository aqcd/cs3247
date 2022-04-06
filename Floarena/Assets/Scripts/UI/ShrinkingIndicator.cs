using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ShrinkingIndicator : NetworkBehaviour {

    public TMP_Text text;
    private Vector3 startPos;
    public float maxHeight = 10f;
    public float maxFontSize = 2f;
    private float startFontSize;

    [ClientRpc]
    public void StartAnim(string indicatorVal) {
        text.text = indicatorVal;
        
        // Start off with damage indicator being invisible
        Color origColor = text.color;
        origColor.a = 0;
        text.color = origColor;

        startPos = transform.position;
        startFontSize = text.fontSize;

        StartCoroutine(FadeInCoroutine());  
    }

    IEnumerator FadeInCoroutine() {
        float startTime = Time.time;
        float curTime = startTime;

        while (true) {
            float val = Mathf.Sin((curTime - startTime) / 0.2f); // Scale sin from 0 to 1
            Color curColor = text.color;

            // Fade in the prefab
            curColor.a += 0.1f;
            text.color = curColor;

            // Have the prefab rise up
            transform.position = new Vector3(startPos.x, startPos.y + val * maxHeight, startPos.z + val * maxHeight);

            // Have prefab's font size increase over time
            text.fontSize = val * maxFontSize;

            if (val >= 0.8f) {
                break;
            }

            yield return new WaitForSeconds(0.01f);
            curTime = Time.time;
        }

        // Fade out the indicator
        StartCoroutine(FadeOutCoroutine());
    }
 
    IEnumerator FadeOutCoroutine() {
        float startTime = Time.time;
        float curTime = startTime;

        while (true) {
            Color curColor = text.color;
            curColor.a -= 0.1f;
            text.color = curColor;

            if (text.color.a <= 0) {
                break;
            }

            yield return new WaitForSeconds(0.01f);
            curTime = Time.time;
        }

        Destroy(gameObject);
    }
}
