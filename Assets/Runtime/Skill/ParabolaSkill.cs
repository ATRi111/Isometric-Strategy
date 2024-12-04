using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "抛物线型技能", menuName = "技能/抛物线型技能", order = -1)]
public class ParabolaSkill : ProjectileSkill
{
    public static float DefaultHeight = 5f;
    public static float Gravity => GridPhysics.settings.gravity;

    public List<float> angles = new() { 0f, 15f, 30f, 45f, 60f, 75f };
    public float maxSpeed;

    public override GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, Vector3Int target, List<Vector3> trajectory)
    {
        GridObject aimedObject = igm.GetObjectXY((Vector2Int)target);
        GridObject gridObject = null;
        for (int i = 0; i < angles.Count; i++)
        {
            bool feasible = GridPhysics.InitialVelocityOfParabola(from, to, angles[i] * Mathf.Deg2Rad, Gravity, out Vector3 velocity, out float _);
            if (!feasible || velocity.magnitude > maxSpeed)
                velocity = maxSpeed * velocity.normalized;
            gridObject = igm.ParabolaCast(from, velocity, Gravity, trajectory);
            if (gridObject == aimedObject)
                return gridObject;
        }
        return gridObject;
    }

    /// <summary>
    /// 根据angles和maxSpeed求水平面内最大飞行距离
    /// </summary>
    public float MaxProjectDistance()
    {
        float max = 0;
        float g = GridPhysics.settings.gravity;
        for (int i = 0; i < angles.Count; i++)
        {
            max = Mathf.Max(max, GridPhysics.ProjectDistanceOfParabola(maxSpeed, angles[i], g, -DefaultHeight));
        }
        return max;
    }
}
