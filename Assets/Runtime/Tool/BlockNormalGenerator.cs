using MyTool;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockNormalGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    public static Color NormalToColor(Vector3 normal)
    {
        normal = normal.normalized;
        normal = 0.5f * normal + 0.5f * Vector3.one;
        return new Color(normal.x, normal.y, normal.z, 1);
    }

    public List<Vector3> normals;
    public List<Vector2> texcoords;
    public List<Vector3Int> indices;

    public SpriteRenderer spriteRenderer;

    public Vector3 CalcNormal(Vector2 uv)
    {
        for (int i = 0; i < indices.Count; i++)
        {
            Vector3Int v = indices[i];
            Vector2 A = texcoords[v.x];
            Vector2 B = texcoords[v.y];
            Vector2 C = texcoords[v.z];
            Vector3 bary = GeometryTool.BarycentricCoordinates(A, B, C, uv);
            if (bary.x >= 0 && bary.y >= 0 && bary.z >= 0)
            {
                Vector3 normal = bary.x * normals[v.x] + bary.y * normals[v.y] + bary.z * normals[v.z];
                return normal;
            }
        }
        return default;
    }

    public Vector2 LocalToUV(Vector3 local)
    {
        Sprite sprite = spriteRenderer.sprite;
        Bounds bounds = sprite.bounds;
        float u = (local.x - bounds.min.x) / bounds.size.x;
        float v = (local.y - bounds.min.y) / bounds.size.y;
        return new Vector2(u, v);
    }

    public NormalVertex[] UpdateVertex()
    {
        NormalVertex[] vertices = GetComponentsInChildren<NormalVertex>();
        texcoords.Clear();
        normals.Clear();
        for (int i = 0; i < vertices.Length; i++)
        {
            normals.Add(vertices[i].normal);
            texcoords.Add(LocalToUV(vertices[i].transform.localPosition));
        }
        return vertices;
    }

    public void Generate()
    {
        Sprite sprite = spriteRenderer.sprite;
        int width = sprite.texture.width;
        int height = sprite.texture.height;
        Texture2D normalMap = new(width, height, TextureFormat.RGBAHalf, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
        };

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 uv = new Vector2((x + 0.5f) / width, (y + 0.5f) / height);
                Vector3 normal = CalcNormal(uv);
                Color color = NormalToColor(normal);
                normalMap.SetPixel(x, y, color);
            }
        }
        normalMap.Apply();
        TextureTool.CreateImage(Application.dataPath + "\\Art\\Normal\\Normal.png", normalMap);
        AssetDatabase.Refresh();
        Destroy(normalMap);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            UpdateVertex();
            Generate();
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            NormalVertex[] vertices = UpdateVertex();
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 normal = normals[i];
                Color color = NormalToColor(normal);
                Gizmos.color = color;
                Gizmos.DrawSphere(vertices[i].transform.position, 0.005f);
            }
        }
    }
#endif
}
