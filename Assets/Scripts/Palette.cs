using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    public int colorIndex = 0;
    [Serializable]
    public enum Theme
    {
        Level1,
        Earthy,
        Cake,
        Radiant,
        Wood,
        City
    };

    [SerializeField]
    private Theme currentTheme;

    Color[] colors;
    PaletteColor[] palettesInstanced;
    GameManager gameManager;

    // public int selectedColorIndex = 0;

    void Start()
    {
        InitializeColors();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        palettesInstanced = new PaletteColor[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            GameObject colorObj = Instantiate(Resources.Load<GameObject>("Prefabs/PaletteColor"), transform);
            PaletteColor paletteColor = colorObj.GetComponent<PaletteColor>();
            palettesInstanced[i] = paletteColor;
            if (paletteColor != null)
            {
                paletteColor.SetColor(colors[i]);
            }
        }
    }
    private void InitializeColors()
    {
        switch (currentTheme)
        {
            case Theme.Level1:
                colors = new Color[]
                {
                    // Neutros
                    new Color(0.9f, 0.9f, 0.9f),   // Blanco suave
                    new Color(0.5f, 0.5f, 0.5f),   // Gris medio
                    new Color(0.25f, 0.25f, 0.25f),// Gris oscuro
                    new Color(0.1f, 0.1f, 0.1f),   // Negro suave

                    // Rojos
                    new Color(0.36f, 0.20f, 0.09f),  // Madera
                    new Color(0.6f, 0.0f, 0.0f),   // Rojo oscuro
                    new Color(0.9f, 0.1f, 0.1f),   // Rojo intenso
                    new Color(1.0f, 0.4f, 0.4f),   // Rojo pastel

                    // Naranjas
                    new Color(1.0f, 0.55f, 0.0f),  // Naranja brillante
                    new Color(1.0f, 0.75f, 0.5f),  // Durazno claro

                    // Amarillos
                    new Color(1.0f, 1.0f, 0.6f),   // Amarillo pastel
                    new Color(1.0f, 0.9f, 0.0f),   // Amarillo intenso
                    new Color(0.85f, 0.75f, 0.0f), // Mostaza

                    // Verdes
                    new Color(0.1f, 0.7f, 0.1f),   // Verde césped
                    new Color(0.0f, 0.4f, 0.0f),   // Verde bosque
                    new Color(0.5f, 1.0f, 0.5f),   // Verde menta

                    // Azules
                    new Color(0.5f, 0.8f, 1.0f),   // Azul pastel
                    new Color(0.0f, 0.5f, 1.0f),   // Azul cielo
                    new Color(0.0f, 0.0f, 0.6f),   // Azul marino

                    // Violetas
                    new Color(0.6f, 0.2f, 0.8f),   // Violeta fuerte
                    new Color(0.8f, 0.6f, 1.0f),   // Lavanda
                    new Color(1.0f, 0.7f, 0.9f),   // Rosa chicle
                };
                break;
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
        ManageIndex();
        ManageColorsPositions();
    }

    private void ManageColorsPositions()
    {
        
    }

    void ManageColorIndex()
    {
        for (int i = 0; i < palettesInstanced.Length; i++)
        {
            PaletteColor color = palettesInstanced[i];
            Transform child = color.transform.GetChild(0);

            // Lerp position
            Vector3 currentPos = child.localPosition;
            Vector3 targetPos = new Vector3(0f, i == colorIndex ? -50f : 0f, 0f);
            child.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * 10f);

            // Lerp rotation
            Quaternion currentRot = child.localRotation;
            Quaternion targetRot = Quaternion.Euler(0f, 0f, i == colorIndex ? -50f : 0f);
            child.localRotation = Quaternion.Lerp(currentRot, targetRot, Time.deltaTime * 10f);

            // Lerp scale
            Vector3 currentScale = child.localScale;
            Vector3 targetScale = i == colorIndex ? Vector3.one * 1.2f : Vector3.one;
            child.localScale = Vector3.Lerp(currentScale, targetScale, Time.deltaTime * 10f);
        }
            
    }

    void ManageIndex()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f || Input.GetKeyDown(KeyCode.RightArrow))
        {
            colorIndex = (colorIndex + 1) % colors.Length;
        }
        else if (scroll < 0f || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            colorIndex = (colorIndex - 1 + colors.Length) % colors.Length;
        }

    }

    public Color GetSelectedColor()
    {
        return colors[gameManager.level != 7? colorIndex : 3];
    }
}
