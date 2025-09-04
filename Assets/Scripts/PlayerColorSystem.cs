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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f)) // 100 = distancia máxima del raycast
            {
                // Intentamos obtener el componente ColorExpansion del objeto golpeado
                ColorExpansion ce = hit.collider.GetComponent<ColorExpansion>();
                if (ce != null)
                {
                    ce.StartEffect(hit.point, palette.GetSelectedColor());
                }
                else
                {
                    Debug.Log("El objeto golpeado no tiene un componente ColorExpansion.");
                }
            }
        }
    }
}
