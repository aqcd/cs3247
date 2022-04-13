using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace Mirror {
    public class PingDisplay : MonoBehaviour {
        public TMP_Text display;

        void Start() {
            NetworkTime.PingFrequency = 0.2f;
            NetworkTime.PingWindowSize = 3;
            StartCoroutine(PingCoroutine());
        }

        IEnumerator PingCoroutine() {
            while (true) {
                int pingMS = (int)Math.Round(NetworkTime.rtt * 1000);
                string pingString = pingMS + "ms";
                display.text = pingString;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

}
