#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Interpolators = UnityEngine.Splines.Interpolators;

namespace Unity.Splines.Examples
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SplineContainer), typeof(MeshRenderer), typeof(MeshFilter))]
    public class RoadBuilder : MonoBehaviour
    {
        [SerializeField]
        List<SplineData<float>> m_Widths = new List<SplineData<float>>();

        public List<SplineData<float>> Widths
        {
            get
            {
                foreach (var width in m_Widths)
                {
                    if (width.DefaultValue == 0)
                        width.DefaultValue = 1f;
                }

                return m_Widths;
            }
        }

        [SerializeField]
        SplineContainer m_Spline;

        public SplineContainer Container
        {
            get
            {
                if (m_Spline == null)
                    m_Spline = GetComponent<SplineContainer>();

                return m_Spline;
            }
            set => m_Spline = value;
        }

        [SerializeField]
        int m_SegmentsPerMeter = 1;

        [SerializeField]
        Mesh m_Mesh;

        [SerializeField]
        float m_TextureScale = 1f;

        public IReadOnlyList<Spline> splines => LoftSplines;

        public IReadOnlyList<Spline> LoftSplines
        {
            get
            {
                if (m_Spline == null)
                    m_Spline = GetComponent<SplineContainer>();

                if (m_Spline == null)
                {
                    Debug.LogError("Cannot loft road mesh because Spline reference is null");
                    return null;
                }

                return m_Spline.Splines;
            }
        }

        [Obsolete("Use LoftMesh instead.", false)]
        public Mesh mesh => LoftMesh;
        public Mesh LoftMesh
        {
            get
            {
                if (m_Mesh != null)
                    return m_Mesh;

                m_Mesh = new Mesh();
                GetComponent<MeshRenderer>().sharedMaterial = Resources.Load<Material>("Materials/ColorExpansion");
                return m_Mesh;
            }
        }

        [Obsolete("Use SegmentsPerMeter instead.", false)]
        public int segmentsPerMeter => SegmentsPerMeter;
        public int SegmentsPerMeter => Mathf.Min(10, Mathf.Max(1, m_SegmentsPerMeter));

        List<Vector3> m_Positions = new List<Vector3>();
        List<Vector3> m_Normals = new List<Vector3>();
        List<Vector2> m_Textures = new List<Vector2>();
        List<int> m_Indices = new List<int>();
        bool m_LoftRoadsRequested = false;

        void Update()
        {
            if (m_LoftRoadsRequested)
            {
                LoftAllRoads();
                m_LoftRoadsRequested = false;
            }
        }

        public void OnEnable()
        {
            // Avoid to point to an existing instance when duplicating the GameObject
            if (m_Mesh != null)
                m_Mesh = null;

            if (m_Spline == null)
                m_Spline = GetComponent<SplineContainer>();

            LoftAllRoads();

#if UNITY_EDITOR
            EditorSplineUtility.AfterSplineWasModified += OnAfterSplineWasModified;
            EditorSplineUtility.RegisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
            Undo.undoRedoPerformed += LoftAllRoads;
#endif

            SplineContainer.SplineAdded += OnSplineContainerAdded;
            SplineContainer.SplineRemoved += OnSplineContainerRemoved;
            SplineContainer.SplineReordered += OnSplineContainerReordered;
            Spline.Changed += OnSplineChanged;
        }

        public void OnDisable()
        {
#if UNITY_EDITOR
            EditorSplineUtility.AfterSplineWasModified -= OnAfterSplineWasModified;
            EditorSplineUtility.UnregisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
            Undo.undoRedoPerformed -= LoftAllRoads;
#endif

            if (m_Mesh != null)
#if  UNITY_EDITOR
                DestroyImmediate(m_Mesh);
#else
                Destroy(m_Mesh);
#endif

            SplineContainer.SplineAdded -= OnSplineContainerAdded;
            SplineContainer.SplineRemoved -= OnSplineContainerRemoved;
            SplineContainer.SplineReordered -= OnSplineContainerReordered;
            Spline.Changed -= OnSplineChanged;
        }

        void OnSplineContainerAdded(SplineContainer container, int index)
        {
            if (container != m_Spline)
                return;

            if (m_Widths.Count < LoftSplines.Count)
            {
                var delta = LoftSplines.Count - m_Widths.Count;
                for (var i = 0; i < delta; i++)
                {
#if  UNITY_EDITOR
                    Undo.RecordObject(this, "Modifying Widths SplineData");
#endif
                    m_Widths.Add(new SplineData<float>() { DefaultValue = 1f });
                }
            }

            LoftAllRoads();
        }

        void OnSplineContainerRemoved(SplineContainer container, int index)
        {
            if (container != m_Spline)
                return;

            if (index < m_Widths.Count)
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Modifying Widths SplineData");
#endif
                m_Widths.RemoveAt(index);
            }

            LoftAllRoads();
        }

        void OnSplineContainerReordered(SplineContainer container, int previousIndex, int newIndex)
        {
            if (container != m_Spline)
                return;

            LoftAllRoads();
        }

        void OnAfterSplineWasModified(Spline s)
        {
            if (LoftSplines == null)
                return;

            foreach (var spline in LoftSplines)
            {
                if (s == spline)
                {
                    m_LoftRoadsRequested = true;
                    break;
                }
            }
        }

        void OnSplineChanged(Spline spline, int knotIndex, SplineModification modification)
        {
            OnAfterSplineWasModified(spline);
        }

        void OnAfterSplineDataWasModified(SplineData<float> splineData)
        {
            foreach (var width in m_Widths)
            {
                if (splineData == width)
                {
                    m_LoftRoadsRequested = true;
                    break;
                }
            }
        }

        public void LoftAllRoads()
        {
            LoftMesh.Clear();
            m_Positions.Clear();
            m_Normals.Clear();
            m_Textures.Clear();
            m_Indices.Clear();
            m_Positions.Capacity = 0;
            m_Normals.Capacity = 0;
            m_Textures.Capacity = 0;
            m_Indices.Capacity = 0;

            for (var i = 0; i < LoftSplines.Count; i++)
                Loft(LoftSplines[i], i);

            LoftMesh.SetVertices(m_Positions);
            LoftMesh.SetNormals(m_Normals);
            LoftMesh.SetUVs(0, m_Textures);
            LoftMesh.subMeshCount = 1;
            LoftMesh.SetIndices(m_Indices, MeshTopology.Triangles, 0);
            LoftMesh.UploadMeshData(false);

            GetComponent<MeshFilter>().sharedMesh = m_Mesh;
        }

        public void Loft(Spline spline, int widthDataIndex)
        {
            if (spline == null || spline.Count < 2)
                return;

            LoftMesh.Clear();

            float length = spline.GetLength();
            if (length <= 0.001f)
                return;

            var segmentsPerLength = SegmentsPerMeter * length;
            var segments = Mathf.CeilToInt(segmentsPerLength);
            var segmentStepT = (1f / SegmentsPerMeter) / length;
            var steps = segments + 1;

            // 4 vértices por step (top L/R, bottom L/R)
            var vertexCount = steps * 4;
            var triangleCount = segments * (6 /*top*/ + 6 /*bottom*/ + 6 /*left wall*/ + 6 /*right wall*/);
            var prevVertexCount = m_Positions.Count;

            m_Positions.Capacity += vertexCount;
            m_Normals.Capacity += vertexCount;
            m_Textures.Capacity += vertexCount;
            m_Indices.Capacity += triangleCount;

            float thickness = 1f; // grosor hacia abajo (en metros)
            var t = 0f;

            for (int i = 0; i < steps; i++)
            {
                // pos, dir, up salen como float3
                SplineUtility.Evaluate(spline, t, out var posF, out var dirF, out var upF);

                // Asegurar dirección si vino cero
                if (math.length(dirF) == 0)
                {
                    var nextPos = spline.GetPointAtLinearDistance(t, 0.01f, out _);
                    dirF = math.normalizesafe(nextPos - posF);

                    if (math.length(dirF) == 0)
                    {
                        nextPos = spline.GetPointAtLinearDistance(t, -0.01f, out _);
                        dirF = -math.normalizesafe(nextPos - posF);
                    }

                    if (math.length(dirF) == 0)
                        dirF = new float3(0, 0, 1);
                }

                var scale = transform.lossyScale;
                var tangentF = math.normalizesafe(math.cross(upF, dirF)) *
                               new float3(1f / scale.x, 1f / scale.y, 1f / scale.z);

                // Ancho de la ruta
                var w = 1f;
                if (widthDataIndex < m_Widths.Count)
                {
                    w = m_Widths[widthDataIndex].DefaultValue;
                    if (m_Widths[widthDataIndex] != null && m_Widths[widthDataIndex].Count > 0)
                    {
                        w = m_Widths[widthDataIndex].Evaluate(spline, t, PathIndexUnit.Normalized, new Interpolators.LerpFloat());
                        w = math.clamp(w, .001f, 10000f);
                    }
                }

                // Convertimos a Vector3 para operar sin ambigüedad
                Vector3 pos = (Vector3)posF;
                Vector3 up = (Vector3)upF;
                Vector3 tangent = (Vector3)tangentF;

                // Vértices
                Vector3 leftTop = pos - (tangent * w);
                Vector3 rightTop = pos + (tangent * w);
                Vector3 leftBottom = leftTop - up * thickness;     // <- ahora todo es Vector3
                Vector3 rightBottom = rightTop - up * thickness;

                // Guardar posiciones
                m_Positions.Add(leftTop);      // 0
                m_Positions.Add(rightTop);     // 1
                m_Positions.Add(leftBottom);   // 2
                m_Positions.Add(rightBottom);  // 3

                // Normales (planas: arriba y abajo)
                m_Normals.Add(up);
                m_Normals.Add(up);
                m_Normals.Add(-up);
                m_Normals.Add(-up);

                // UVs (básicos)
                float v = t * m_TextureScale;
                m_Textures.Add(new Vector2(0f, v));
                m_Textures.Add(new Vector2(1f, v));
                m_Textures.Add(new Vector2(0f, v));
                m_Textures.Add(new Vector2(1f, v));

                t = math.min(1f, t + segmentStepT);
            }

            // Triángulos por segmento
            for (int i = 0; i < segments; i++)
            {
                int n0 = prevVertexCount + i * 4;
                int n1 = n0 + 4;

                int lt0 = n0;     // left top
                int rt0 = n0 + 1; // right top
                int lb0 = n0 + 2; // left bottom
                int rb0 = n0 + 3; // right bottom

                int lt1 = n1;
                int rt1 = n1 + 1;
                int lb1 = n1 + 2;
                int rb1 = n1 + 3;

                // Cara superior
                m_Indices.Add(lt0); m_Indices.Add(lt1); m_Indices.Add(rt0);
                m_Indices.Add(rt0); m_Indices.Add(lt1); m_Indices.Add(rt1);

                // Cara inferior (invertida)
                m_Indices.Add(lb0); m_Indices.Add(rb0); m_Indices.Add(lb1);
                m_Indices.Add(rb0); m_Indices.Add(rb1); m_Indices.Add(lb1);

                // Pared izquierda
                m_Indices.Add(lt0); m_Indices.Add(lb0); m_Indices.Add(lt1);
                m_Indices.Add(lb0); m_Indices.Add(lb1); m_Indices.Add(lt1);

                // Pared derecha
                m_Indices.Add(rt0); m_Indices.Add(rt1); m_Indices.Add(rb0);
                m_Indices.Add(rb0); m_Indices.Add(rt1); m_Indices.Add(rb1);
            }
        }

    }
}
