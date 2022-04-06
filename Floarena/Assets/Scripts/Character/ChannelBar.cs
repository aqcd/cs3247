using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChannelBar : MonoBehaviour
{
    public Slider slider;

    public TMP_Text text;
    void Start()
    {
        slider.maxValue = BerryConstants.CHANNEL_DURATION;
        gameObject.SetActive(false);
        text.enabled = false;
    }

    public void EnableBar() {
        gameObject.SetActive(true);
        text.enabled = true;
    }

    public void DisableBar() {
        gameObject.SetActive(false);
        text.enabled = false;
    }

    public void SetChannel(float time) {
        slider.value = time;
    }
}
