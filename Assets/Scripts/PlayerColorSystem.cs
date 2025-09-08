using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorSystem : MonoBehaviour
{
    Camera cam;
    Palette palette;

    void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        palette = GameObject.Find("Palette").GetComponent<Palette>();
    }

    void Update()
    {

        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (leftClick || rightClick)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            int layerMask = ~(1 << 6); // Ignore layer 6 (Player)
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
