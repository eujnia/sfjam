using UnityEngine;
using System.Collections.Generic;
using System;

public class ColorExpansion : MonoBehaviour
{
    public Texture myTexture;
    public bool movingObject;
    public bool disk;
    public bool house;
    Renderer myRenderer;
    Material mat;
    MeshCollider meshCollider;
    float duration;
    float progress;
    bool active;
    public Color actualColor = Color.white;
    public ColorExpansion[] linkedObjects;
    public Color debugMenu;


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

        for (int i = linkedObjects.Length - 1; i >= 0; i--)
        {
            if (linkedObjects[i] == this)
            {
                // Remove the element by creating a new array without it
                var tempList = new List<ColorExpansion>(linkedObjects);
                tempList.RemoveAt(i);
                linkedObjects = tempList.ToArray();
            }
        }
    }

    public void StartEffect(Vector3 origin, Color color, bool stacked = false)
    {
        if (!stacked)
        {
            foreach (ColorExpansion colorExpansion in linkedObjects)
            {
                colorExpansion.StartEffect(colorExpansion.transform.position, color, true);
            }
        }

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
        actualColor = color;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Bounds bounds = mr.bounds;
        float radius = bounds.extents.magnitude;
        radius = radius * (movingObject ? 250f : 2f);

        duration = Mathf.Clamp((radius * 10f) / 20f, 3f, 30f);
        if (movingObject)
            duration = 1f;

        progress = 0;
        active = true;
        mat.SetColor("_BaseColor", mat.GetColor("_NewColor"));
        mat.SetVector("_Point", origin);
        mat.SetFloat("_Radius", radius);
        if (disk)
        {
            mat.SetColor("_NewColor", new Color(0f, 0f, 0f, 0.5f));
            DiskData diskDataFound = FindDiskData();
            mat.SetTexture("_MainTex", diskDataFound.texture);
            Music.Instance.PlaySong(diskDataFound.songName);
        }
        else
        if (house)
        {
            mat.SetColor("_NewColor", new Color(0f, 0f, 0f, 0.5f));
            mat.SetTexture("_MainTex", myTexture);
        }
        else
        {
            mat.SetColor("_NewColor", color);
            if (myTexture != null)
            {
                mat.SetTexture("_MainTex", myTexture);
            }
        }
    }

    private DiskData FindDiskData()
    {
        DiskData diskDataFound = Music.Instance.disksData[0];
        GameObject gob = GameObject.Find("Palette");
        if (gob)
        {
            Palette palette = gob.GetComponent<Palette>();
            int colorID = palette.colorIndex;
            foreach (DiskData diskData in Music.Instance.disksData)
            {
                if (Array.Exists(diskData.colorIDs, id => id == colorID))
                {
                    diskDataFound = diskData;
                    break;
                }
            }
        }
        return diskDataFound;
    }

    void Update()
    {
        if (!active) return;

        float speedMultiplier = (effectQueue.Count > 0) ? 100f : 1f;
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
