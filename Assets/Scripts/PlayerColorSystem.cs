using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorSystem : MonoBehaviour
{
    Camera cam;
    Palette palette;
    GameManager gameManager;

    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        palette = GameObject.Find("Palette").GetComponent<Palette>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {

        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (leftClick || rightClick && !new List<int> { 2, 4, 6, 9, 10 }.Contains(gameManager.level))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            int layerMask = ~((1 << 6) | (1 << 7)); // Ignore layers 6 (Player) and 7
            if (Physics.Raycast(ray, out hit, 200f, layerMask)) // 200 = distancia máxima del raycast
            {
                // Intentamos obtener el componente ColorExpansion del objeto golpeado
                ColorExpansion ce = hit.collider.GetComponent<ColorExpansion>();
                if (ce != null)
                {
                    ce.StartEffect(hit.point, leftClick ? palette.GetSelectedColor() : Color.white);
                }
                else
                {
                    Debug.Log("El objeto golpeado no tiene un componente ColorExpansion.");
                }
            }
        }
    }
}
