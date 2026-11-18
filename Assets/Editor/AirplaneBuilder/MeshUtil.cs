using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class MeshUtil
    {
        public static Mesh CreateBox(Vector3 size, Vector3 offset = default)
        {
            Mesh mesh = new Mesh { name = "FlatBox" };
            Vector3 h = size * 0.5f;

            // 6 faces * 4 vertices = 24 vertices
            Vector3[] v = new Vector3[]
            {
                // Front (+Z)
                offset + new Vector3(-h.x, -h.y,  h.z),
                offset + new Vector3( h.x, -h.y,  h.z),
                offset + new Vector3( h.x,  h.y,  h.z),
                offset + new Vector3(-h.x,  h.y,  h.z),
                // Back (-Z)
                offset + new Vector3( h.x, -h.y, -h.z),
                offset + new Vector3(-h.x, -h.y, -h.z),
                offset + new Vector3(-h.x,  h.y, -h.z),
                offset + new Vector3( h.x,  h.y, -h.z),
                // Left (-X)
                offset + new Vector3(-h.x, -h.y, -h.z),
                offset + new Vector3(-h.x, -h.y,  h.z),
                offset + new Vector3(-h.x,  h.y,  h.z),
                offset + new Vector3(-h.x,  h.y, -h.z),
                // Right (+X)
                offset + new Vector3( h.x, -h.y,  h.z),
                offset + new Vector3( h.x, -h.y, -h.z),
                offset + new Vector3( h.x,  h.y, -h.z),
                offset + new Vector3( h.x,  h.y,  h.z),
                // Top (+Y)
                offset + new Vector3(-h.x,  h.y,  h.z),
                offset + new Vector3( h.x,  h.y,  h.z),
                offset + new Vector3( h.x,  h.y, -h.z),
                offset + new Vector3(-h.x,  h.y, -h.z),
                // Bottom (-Y)
                offset + new Vector3(-h.x, -h.y, -h.z),
                offset + new Vector3( h.x, -h.y, -h.z),
                offset + new Vector3( h.x, -h.y,  h.z),
                offset + new Vector3(-h.x, -h.y,  h.z)
            };

            int[] tris = new int[36];
            for (int f = 0; f < 6; f++)
            {
                int vi = f * 4;
                int ti = f * 6;
                tris[ti + 0] = vi + 0;
                tris[ti + 1] = vi + 2;
                tris[ti + 2] = vi + 1;
                tris[ti + 3] = vi + 0;
                tris[ti + 4] = vi + 3;
                tris[ti + 5] = vi + 2;
            }

            mesh.vertices = v;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh CreateQuad(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, bool doubleSided = true)
        {
            Mesh mesh = new Mesh { name = "FlatQuad" };
            if (doubleSided)
            {
                mesh.vertices = new Vector3[] { p0, p1, p2, p3, p0, p3, p2, p1 };
                mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7 };
            }
            else
            {
                mesh.vertices = new Vector3[] { p0, p1, p2, p3 };
                mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            }
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh CreatePrism(int sides, float radius, float height, Vector3 offset = default)
        {
            Mesh mesh = new Mesh { name = $"Prism_{sides}" };
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            float halfH = height * 0.5f;
            float step = Mathf.PI * 2f / sides;

            // Side faces
            for (int i = 0; i < sides; i++)
            {
                float a0 = i * step;
                float a1 = (i + 1) * step;

                Vector3 b0 = offset + new Vector3(Mathf.Cos(a0) * radius, -halfH, Mathf.Sin(a0) * radius);
                Vector3 b1 = offset + new Vector3(Mathf.Cos(a1) * radius, -halfH, Mathf.Sin(a1) * radius);
                Vector3 t0 = offset + new Vector3(Mathf.Cos(a0) * radius,  halfH, Mathf.Sin(a0) * radius);
                Vector3 t1 = offset + new Vector3(Mathf.Cos(a1) * radius,  halfH, Mathf.Sin(a1) * radius);

                AddQuad(verts, tris, b0, t0, t1, b1);
            }

            // Top and bottom caps
            Vector3 topCenter = offset + new Vector3(0, halfH, 0);
            Vector3 botCenter = offset + new Vector3(0, -halfH, 0);
            for (int i = 0; i < sides; i++)
            {
                float a0 = i * step;
                float a1 = (i + 1) * step;

                Vector3 t0 = offset + new Vector3(Mathf.Cos(a0) * radius, halfH, Mathf.Sin(a0) * radius);
                Vector3 t1 = offset + new Vector3(Mathf.Cos(a1) * radius, halfH, Mathf.Sin(a1) * radius);
                AddTriangle(verts, tris, topCenter, t0, t1);

                Vector3 b0 = offset + new Vector3(Mathf.Cos(a0) * radius, -halfH, Mathf.Sin(a0) * radius);
                Vector3 b1 = offset + new Vector3(Mathf.Cos(a1) * radius, -halfH, Mathf.Sin(a1) * radius);
                AddTriangle(verts, tris, botCenter, b1, b0);
            }

            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static void AddTriangle(List<Vector3> verts, List<int> tris, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            int idx = verts.Count;
            verts.Add(p0);
            verts.Add(p1);
            verts.Add(p2);
            tris.Add(idx);
            tris.Add(idx + 1);
            tris.Add(idx + 2);
        }

        public static void AddQuad(List<Vector3> verts, List<int> tris, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            int idx = verts.Count;
            verts.Add(p0);
            verts.Add(p1);
            verts.Add(p2);
            verts.Add(p3);

            tris.Add(idx + 0);
            tris.Add(idx + 1);
            tris.Add(idx + 2);
            tris.Add(idx + 0);
            tris.Add(idx + 2);
            tris.Add(idx + 3);
        }

        public static void AddDoubleSidedQuad(List<Vector3> verts, List<int> tris, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            AddQuad(verts, tris, p0, p1, p2, p3);
            AddQuad(verts, tris, p0, p3, p2, p1);
        }

        public static GameObject CreateMeshObject(string name, Mesh mesh, Material mat, Transform parent, bool addBoxCollider = false)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;

            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;

            if (addBoxCollider)
            {
                BoxCollider col = go.AddComponent<BoxCollider>();
                col.center = mesh.bounds.center;
                col.size = mesh.bounds.size;
            }

            return go;
        }

        public static GameObject CreateBoxObject(string name, Vector3 size, Material mat, Transform parent, Vector3 localPos = default, bool addCollider = false)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.sharedMesh = CreateBox(size);

            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;

            if (addCollider)
            {
                BoxCollider col = go.AddComponent<BoxCollider>();
                col.size = size;
            }

            return go;
        }
    }
}
