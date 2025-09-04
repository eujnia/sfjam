using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteColor : MonoBehaviour
{
    [SerializeField]
    public Color color;
    void Start()
    {
        GetComponentInChildren<UnityEngine.UI.Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
