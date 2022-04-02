using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelBar : MonoBehaviour
{
    public Slider slider;
    void Start()
    {
        slider.maxValue = BerryConstants.CHANNEL_DURATION;
        gameObject.SetActive(false);
    }

    public void EnableBar() {
        gameObject.SetActive(true);
    }

    public void DisableBar() {
        gameObject.SetActive(false);
    }

    public void SetChannel(float time) {
        slider.value = time;
    }
}
