using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static int SampleDistance = 20;
    public static Vector3 CellSize = new(1f, 1f, 0.5f);

    public static Vector3Int[] XYDirections =
    {
        Vector3Int.up,
        Vector3Int.left + Vector3Int.up,
        Vector3Int.left,
        Vector3Int.left + Vector3Int.down,
        Vector3Int.down,
        Vector3Int.right + Vector3Int.down,
        Vector3Int.right,
        Vector3Int.right + Vector3Int.up,
    };

    public static Vector3Int[] XZDirections =
    {
        Vector3Int.forward,
        Vector3Int.right + Vector3Int.forward,
        Vector3Int.right,
        Vector3Int.right + Vector3Int.back,
        Vector3Int.back,
        Vector3Int.left + Vector3Int.back,
        Vector3Int.left,
        Vector3Int.left + Vector3Int.forward,
    };

    public static Vector3Int[] YZDirections =
    {
        Vector3Int.forward,
        Vector3Int.up + Vector3Int.forward,
        Vector3Int.up,
        Vector3Int.up + Vector3Int.back,
        Vector3Int.back,
        Vector3Int.down + Vector3Int.back,
        Vector3Int.down,
        Vector3Int.down + Vector3Int.forward,
    };

    private IsometricGridManager igm;
    private readonly HashSet<Vector3Int> objectCache = new();

    public Color radiance_up;
    public Color radiance_left;
    public Color radiance_right;
    public float projectShadowIntensity = 0.5f;

    public Color GetRadiance(ShadowVertex vertex)
    {
        return AmbientOcculasion(vertex);
    }

    public float Sample(Vector3Int p)
    {
        return objectCache.Contains(p) ? 1f : 0f;
    }

    public bool VisibleCheck(Vector3Int position)
    {
        return !objectCache.Contains(position + Vector3Int.forward)
            || !objectCache.Contains(position + Vector3Int.left)
            || !objectCache.Contains(position + Vector3Int.down);
    }

    public Color AmbientOcculasion(ShadowVertex vertex)
    {
        Vector3Int[] directions;
        float visibility = 0f;
        if (vertex.cellNormal.x != 0)
            directions = YZDirections;
        else if (vertex.cellNormal.y != 0)
            directions = XZDirections;
        else if (vertex.cellNormal.z != 0)
            directions = XYDirections;
        else
            return Color.clear;

        for (int i = 0; i < directions.Length; i++)
        {
            visibility += AmbientOcculasion(vertex, directions[i]);
        }
        visibility /= directions.Length;
        Color radiance;
        if (vertex.cellNormal == Vector3Int.forward)
            radiance = radiance_up;
        else if (vertex.cellNormal == Vector3Int.left)
            radiance = radiance_left;
        else if (vertex.cellNormal == Vector3Int.down)
            radiance = radiance_right;
        else
            throw new System.ArgumentException();
        float alpha = radiance.a;
        radiance *= visibility;
        radiance = new Color(radiance.r, radiance.g, radiance.b, alpha);
        return radiance;
    }

    private float AmbientOcculasion(ShadowVertex vertex, Vector3Int direction)
    {
        float minVisibility = 1f;
        Vector3Int basePosition = vertex.cellPosition;
        float unitDistance = new Vector3(CellSize.x * direction.x, CellSize.y * direction.y, CellSize.z * direction.z).magnitude;
        float unitHeight = vertex.cellNormal.z != 0 ? 0.5f : 1f;
        for (int i = 0; i < SampleDistance; i++)
        {
            basePosition += direction;
            float h = GetHeight(basePosition, vertex.cellNormal) * unitHeight;
            float d = (i + 0.5f) * unitDistance;
            if (h > 0)
            {
                float angle = Mathf.Atan(h / d);
                minVisibility = Mathf.Min(minVisibility, 1f - angle * 2 / Mathf.PI);
            }
        }
        return minVisibility;
    }

    private float GetHeight(Vector3Int basePosition, Vector3Int normal)
    {
        int h = 0;
        while (objectCache.Contains(basePosition))
        {
            basePosition += normal;
            h++;
        }
        return h - 1;
    }

    private const float ShrinkRatio = 100.0f;

    public Texture2D GenerateAO(Vector3Int position, Vector3Int normal)
    {
        static Vector2 UpdateMax(Vector2 prev, Vector2 current, float offset)
        {
            if (current.y / (current.x + offset) > prev.y / (prev.x + offset))
                return current;
            return prev;
        }

        Vector3Int[] directions;
        float distanceOffset = 0.499f;
        float unitHeight = normal.z != 0 ? 0.5f : 1f;
        if (normal.x != 0)
        {
            directions = YZDirections;
        }
        else if (normal.y != 0)
        {
            directions = XZDirections;
        }
        else if (normal.z != 0)
        {
            directions = XYDirections;
        }
        else
            throw new System.ArgumentException();

        Texture2D AO = new(3, 3, TextureFormat.RGBAFloat, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
        };
        AO.SetPixel(1, 1, new Color(1, 0, 1, 0));
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 max1 = Vector2.right;
            Vector2 max2 = Vector2.right;
            Vector3Int direction = directions[i];
            float unitDistance = new Vector3(CellSize.x * direction.x, CellSize.y * direction.y, CellSize.z * direction.z).magnitude;
            Vector3Int currentPosition = position;

            for (int j = 0; j < SampleDistance; j++)
            {
                currentPosition += direction;
                float h = GetHeight(currentPosition, normal) * unitHeight;
                float d = (j + 0.5f) * unitDistance;
                if (h > 0)
                {
                    Vector2 current = new(d, h);
                    max1 = UpdateMax(max1, current, -distanceOffset);
                    max2 = UpdateMax(max2, current, distanceOffset);
                }
            }
            Color color = new(max1.x / ShrinkRatio, max1.y / ShrinkRatio, max2.x / ShrinkRatio, max2.y / ShrinkRatio);

            int x = XYDirections[i].x + 1;
            int y = XYDirections[i].y + 1;
            AO.SetPixel(x, y, color);
        }
        AO.Apply();
        return AO;
    }

    private void Awake()
    {
        igm = GetComponent<IsometricGridManager>();
        FaceShadowGenerator[] shadows = GetComponentsInChildren<FaceShadowGenerator>();
        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadows[i].transform.position);
            objectCache.Add(cellPosition);
        }
    }
}
