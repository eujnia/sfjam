using UnityEngine;
using System.Collections.Generic;

public class ColorExpansion : MonoBehaviour
{
    public Texture myTexture;
    Renderer myRenderer;
    Material mat;
    MeshCollider meshCollider;
    float duration;

    float progress;
    bool active;

    // Cola de efectos pendientes
    private Queue<(Vector3 origin, Color color)> effectQueue = new Queue<(Vector3, Color)>();

    private void Start()
    {
        if (!myTexture)
            myTexture = Resources.Load<Texture>("Textures/Grass1.png");

        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
            meshCollider = gameObject.AddComponent<MeshCollider>();

        myRenderer = GetComponent<Renderer>();
        myRenderer.material = Resources.Load<Material>("Materials/ColorExpansion");
        mat = myRenderer.material;
        mat.SetFloat("_Progress", 0);
        active = false;
        progress = 0;
    }

    public void StartEffect(Vector3 origin, Color color)
    {
        // Si ya hay un efecto activo → encolamos
        if (active)
        {
            effectQueue.Enqueue((origin, color));
            return;
        }

        PlayEffect(origin, color);
    }

    private void PlayEffect(Vector3 origin, Color color)
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
        duration = Mathf.Clamp((radius * 10f) / 20f, 3f, 10f);

        if (myTexture != null)
        {
            mat.SetTexture("_MainTex", myTexture);
        }
    }

    void Update()
    {
        if (!active) return;

        float speedMultiplier = (effectQueue.Count > 0) ? 20f : 1f;
        progress += Time.deltaTime / duration * speedMultiplier;
        mat.SetFloat("_Progress", progress);

        if (progress >= 1f)
        {
            active = false;

            // Si hay pendientes, arrancamos el siguiente
            if (effectQueue.Count > 0)
            {
                var next = effectQueue.Dequeue();
                PlayEffect(next.origin, next.color);
            }
        }
    }
}
