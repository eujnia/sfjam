using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteColor : MonoBehaviour
{
    public void SetColor(Color newColor)
    {
        transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().color = newColor;
    }
}
