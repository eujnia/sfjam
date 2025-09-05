using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    [Serializable]
    public enum Theme
    {
        Earthy,
        Cake,
        Radiant,
        Wood,
        City
    };

    [SerializeField]
    private Theme currentTheme;

    Color[] colors;

    // public int selectedColorIndex = 0;

    void Start()
    {
        InitializeColors();
        foreach (Color color in colors)
        {
            GameObject colorObj = Instantiate(Resources.Load<GameObject>("Prefabs/PaletteColor"), transform);
            PaletteColor paletteColor = colorObj.GetComponent<PaletteColor>();
            if (paletteColor != null)
            {
                paletteColor.color = color;
            }
        }
    }
    private void InitializeColors()
    {
        switch (currentTheme)
        {
            case Theme.Earthy:
                colors = new Color[]
                {
                // Deep, earthy reds and oranges
                new Color(0.6f, 0.2f, 0.2f),   // Muted Brick Red
                new Color(0.7f, 0.4f, 0.1f),   // Terracotta Orange
                new Color(0.8f, 0.5f, 0.2f),   // Burnt Sienna

                // Warm, golden yellows
                new Color(0.9f, 0.7f, 0.2f),   // Goldenrod
                new Color(0.9f, 0.8f, 0.4f),   // Mustard Yellow

                // Muted greens and blues for subtle contrast
                new Color(0.5f, 0.6f, 0.4f),   // Sage Green
                new Color(0.3f, 0.4f, 0.5f),   // Dusty Blue

                // Neutrals for balance and light
                new Color(0.9f, 0.8f, 0.7f),   // Off-white or Cream
                new Color(0.3f, 0.3f, 0.3f),   // Charcoal Gray
                new Color(0.6f, 0.5f, 0.4f)    // Warm Taupe
                };
                break;
            case Theme.Cake:
                colors = new Color[]
                {
                    // Tonos pasteles suaves
                    new Color(0.9f, 0.7f, 0.7f),   // Rosa suave
                    new Color(0.9f, 0.8f, 0.6f),   // Durazno pálido
                    new Color(0.9f, 0.9f, 0.7f),   // Amarillo limón
                    new Color(0.7f, 0.9f, 0.7f),   // Verde menta
                    new Color(0.7f, 0.9f, 0.9f),   // Turquesa claro
                    new Color(0.7f, 0.7f, 0.9f),   // Lila suave
                    new Color(0.8f, 0.7f, 0.9f),   // Lavanda

                    // Neutros para contraste suave
                    new Color(0.95f, 0.95f, 0.95f), // Blanco hueso
                    new Color(0.6f, 0.6f, 0.6f),    // Gris perla
                    new Color(0.8f, 0.8f, 0.85f)    // Gris azulado
                };
                break;
            case Theme.Radiant:
                colors = new Color[]
                {
                    // Colores primarios y secundarios vibrantes
                    new Color(1.0f, 0.0f, 0.0f),   // Rojo puro
                    new Color(1.0f, 0.5f, 0.0f),   // Naranja brillante
                    new Color(1.0f, 1.0f, 0.0f),   // Amarillo puro
                    new Color(0.0f, 1.0f, 0.0f),   // Verde puro
                    new Color(0.0f, 1.0f, 1.0f),   // Cian
                    new Color(0.0f, 0.0f, 1.0f),   // Azul puro
                    new Color(0.5f, 0.0f, 1.0f),   // Púrpura

                    // Acentos vivos
                    new Color(1.0f, 0.0f, 1.0f),   // Magenta brillante
                    new Color(0.2f, 0.8f, 0.7f),   // Turquesa vibrante
                    new Color(1.0f, 0.7f, 0.8f)    // Fucsia
                };
                break;
            case Theme.Wood:
                colors = new Color[]
                {
                    // Tonos de madera
                    new Color(0.9f, 0.75f, 0.6f),  // Madera de pino (clara)
                    new Color(0.7f, 0.5f, 0.3f),   // Madera de roble (media)
                    new Color(0.5f, 0.3f, 0.1f),   // Nogal (oscura)
                    new Color(0.35f, 0.2f, 0.05f)  // Ébano (muy oscura)
                };
                break;
            case Theme.City:
                colors = new Color[]
                {
                    // Rojo Señal (tipo STOP)
                    new Color(0.8f, 0.1f, 0.1f),

                    // Naranja Tránsito (tipo Cono)
                    new Color(1.0f, 0.6f, 0.0f),

                    // Amarillo Alta Visibilidad (tipo Cinta Policial)
                    new Color(1.0f, 0.9f, 0.0f),

                    // Verde Contenedor
                    new Color(0.1f, 0.5f, 0.1f),

                    // Gris Tacho de Basura
                    new Color(0.4f, 0.4f, 0.4f),

                    // Gris Asfalto (un gris más oscuro para contraste)
                    new Color(0.25f, 0.25f, 0.25f)
                };
                break;
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
