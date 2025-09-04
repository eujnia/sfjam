using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ColorExpansion : MonoBehaviour
{
    public Material mat;
    public float duration;

    private float progress;
    private bool active;

    private void Start() {
        if (mat == null)
            mat = GetComponent<Renderer>().material;
        mat.SetFloat("_Progress", 0);
        active = false;
        progress = 0;
    }

    public void StartEffect(Vector3 origin, Color color)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Bounds bounds = mr.bounds;
        float radius = bounds.extents.magnitude;

        progress = 0;
        active = true;
        mat.SetColor("_BaseColor", mat.GetColor("_NewColor"));
        mat.SetVector("_Point", origin);
        mat.SetFloat("_Radius", radius * 2f);
        mat.SetColor("_NewColor", color);
        duration = 5f;
    }

    void Update()
    {
        if (!active) return;

        progress += Time.deltaTime / duration;
        mat.SetFloat("_Progress", progress);

        if (progress >= 1f)
            active = false;
    }
}
