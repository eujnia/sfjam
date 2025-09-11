using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityToggle : MonoBehaviour
{
    UnityEngine.UI.Image slot1;
    UnityEngine.UI.Image slot2;
    UnityEngine.UI.Image slot3;
    UnityEngine.UI.Image slot4;
    void Start()
    {
        slot1 = transform.GetChild(0).GetChild(0).Find("Slot1").GetComponent<UnityEngine.UI.Image>();
        slot2 = transform.GetChild(0).GetChild(0).Find("Slot2").GetComponent<UnityEngine.UI.Image>();
        slot3 = transform.GetChild(0).GetChild(0).Find("Slot3").GetComponent<UnityEngine.UI.Image>();
        slot4 = transform.GetChild(0).GetChild(0).Find("Slot4").GetComponent<UnityEngine.UI.Image>();
        UpdateSlotImages(Config.Instance.data.sensitivity);
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            float sensitivity = Config.Instance.ToggleSensitivity();
            UpdateSlotImages(sensitivity);
        });
    }

    private void UpdateSlotImages(float sensitivity)
    {
        Color c = new Color(1f, 0.98f, 0.60f);
        slot1.color = sensitivity >= 0.5f ? c : Color.gray;
        slot2.color = sensitivity >= 1f ? c : Color.gray;
        slot3.color = sensitivity >= 1.5f ? c : Color.gray;
        slot4.color = sensitivity >= 2f ? c : Color.gray;
    }
}
