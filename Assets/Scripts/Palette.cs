using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    [SerializeField]
    public Color[] colors;

    // public int selectedColorIndex = 0;

    void Start()
    {
        foreach (Color color in colors)
        {
            GameObject colorObj = Instantiate(Resources.Load<GameObject>("PaletteColor"), transform);
            PaletteColor paletteColor = colorObj.GetComponent<PaletteColor>();
            if (paletteColor != null)
            {
                paletteColor.color = color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ManageColorIndex();
        ManageColorsRotation();
    }

    void ManageColorIndex()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.GetChild(transform.childCount - 1).SetSiblingIndex(0);
        }
        else if (scroll < 0f || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.GetChild(0).SetSiblingIndex(transform.childCount - 1);
        }
    }

    void ManageColorsRotation()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            Transform colorTransform = transform.GetChild(i);
            float angle = 360f / colors.Length * i;
            Quaternion targetRotation = Quaternion.Euler(0, -angle, 0);
            colorTransform.localRotation = Quaternion.Lerp(colorTransform.localRotation, targetRotation, Time.deltaTime * 10f);
            float distance = Mathf.Abs(i);
            if (distance > colors.Length / 2)
                distance = colors.Length - distance;
            float scale = Mathf.Lerp(1.2f, 0.7f, distance / (colors.Length / 2f));
            colorTransform.localScale = Vector3.Lerp(colorTransform.localScale, Vector3.one * scale, Time.deltaTime * 10f);

            Vector3 finalPosition = transform.position;
            if (i == 0)
            {
                finalPosition += Vector3.up * 15f;
            }

            colorTransform.position = Vector3.Lerp(colorTransform.position, finalPosition, Time.deltaTime * 10f);
        }

    }
    
    public Color GetSelectedColor()
    {
        return transform.GetChild(0).GetComponent<PaletteColor>().color;
    }
}
