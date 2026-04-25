using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 每个地块有三个可见面；渲染前，判断像素属于哪个面，然后计算该面中的uv
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct GroundFaceInstance
{
    public Vector3 origin;
    public Vector3 uAxis;
    public Vector3 vAxis;
}

static class GroundShadowHelper
{
    //逻辑空间下,Z轴为向上
    public static void GenerateShadowFace(HashSet<Vector3Int> grounds, Vector3Int cellPosition, List<GroundFaceInstance> ret)
    {
        //上表面
        if (!grounds.Contains(cellPosition + Vector3Int.forward))
        {
            ret.Add(new GroundFaceInstance
            {
                origin = cellPosition + Vector3.forward,
                uAxis = Vector3.right,
                vAxis = Vector3.up
            });
        }
        //"左下"表面
        if (!grounds.Contains(cellPosition + Vector3Int.left))
        {
            ret.Add(new GroundFaceInstance
            {
                origin = cellPosition,
                uAxis = Vector3.up,
                vAxis = Vector3.forward
            });
        }
        //"右下"表面
        if (!grounds.Contains(cellPosition + Vector3Int.down))
        {
            ret.Add(new GroundFaceInstance
            {
                origin = cellPosition,
                uAxis = Vector3.right,
                vAxis = Vector3.forward
            });
        }
    }

    public static Mesh CreateFaceMesh()
    {
        Mesh mesh = new()
        {
            name = "UnitQuad",

            vertices = new Vector3[]
            {
                new(0f, 0f, 0f),
                new(1f, 0f, 0f),
                new(0f, 1f, 0f),
                new(1f, 1f, 0f),
            },

            triangles = new int[]
            {
                0, 2, 1,
                2, 3, 1
            },

            uv = new Vector2[]
            {
                new(0f, 0f),
                new(1f, 0f),
                new(0f, 1f),
                new(1f, 1f),
            }
        };

        mesh.RecalculateBounds();
        return mesh;
    }
}

