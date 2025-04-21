using MyTimer;
using MyTool;
using Services.ObjectPools;
using System;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public class ShakeLerp : ILerp<float>
    {
        public const float MinAmplitude = 0.05f;

        public float Value(float origin, float target, float percent, float time, float duration)
        {
            float t = Mathf.Lerp(origin, target, percent);
            float a;
            if (t < 1f)
            {
                t *= Mathf.PI;
                a = 4 * MinAmplitude;
            }
            else if (t < 2f)
            {
                t = (t - 1f) * Mathf.PI;
                a = 2 * MinAmplitude;
            }
            else
            {
                t = (t - 2f) * Mathf.PI;
                a = MinAmplitude;
            }
            return a * Mathf.Sin(t);
        }
    }


    public class ShakeTimer : Timer<float, ShakeLerp>
    {
        
    }

    private Transform shakeTarget;

    private ShakeTimer shakeTimer;
    private Vector3 shakeDirection;
    [SerializeField]
    private float duration;

    public void Initialize(PawnEntity attacker, HPChangeEffect effect)
    {
        if (!effect.WillHappen)
            throw new ArgumentException();

        shakeTarget = effect.victim.GetComponentInChildren<SpriteRenderer>().transform;

        Vector3Int v = effect.victim.GridObject.CellPosition - attacker.GridObject.CellPosition;
        Vector3Int cellDirection = (Vector3Int)EDirectionTool.NearestDirection4((Vector2Int)v);
        shakeDirection = IsometricGridManager.Instance.CellToWorld(cellDirection).normalized;

        float damagePercent = (effect.prev - effect.current) / effect.victim.DefenceComponent.maxHP.CurrentValue;
        if (damagePercent < 0f)
            throw new ArgumentException();
        else if (damagePercent < 0.1f)
            shakeTimer.Initialize(2f, 3f, duration);
        else if (damagePercent < 0.4f)
            shakeTimer.Initialize(1f, 3f, 2 * duration);
        else
            shakeTimer.Initialize(0f, 3f, 3 * duration);
    }

    private void OnTick(float distance)
    {
        shakeTarget.localPosition = distance * shakeDirection;
    }

    private void AfterComplete(float _)
    {
        shakeTarget.transform.localPosition = Vector3.zero;
        MyObject obj = GetComponent<MyObject>();
        obj.Recycle();
    }

    private void Awake()
    {
        shakeTimer = new();
        shakeTimer.OnTick += OnTick;
        shakeTimer.AfterComplete += AfterComplete;
    }
}
