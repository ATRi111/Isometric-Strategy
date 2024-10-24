using MyTimer;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Vector3[] currentRoute;
    [SerializeField]
    protected int currentIndex;
    [SerializeField]
    protected UniformLinearMotion ulm;

    protected virtual void Awake()
    {
        ulm = new UniformLinearMotion();
        ulm.OnTick += OnTick;
        ulm.AfterCompelete += AfterComplete;
    }

    protected virtual void OnDisable()
    {
        ulm.Paused = true;
    }

    public void SetRoute(List<Vector3> route)
    {
        if (route.Count < 2)
            return;
        currentRoute = route.ToArray();
        currentIndex = 1;
        MoveTo(currentRoute[currentIndex]);
    }

    public virtual void ForceComplete()
    {
        ulm.Paused = true;
        transform.position = currentRoute[^1];
    }

    protected virtual void OnTick(Vector3 v)
    {
        transform.position = v;
    }

    protected virtual void AfterComplete(Vector3 v)
    {
        transform.position = v;
        currentIndex++;
        if (currentIndex < currentRoute.Length)
            MoveTo(currentRoute[currentIndex]);
    }

    protected virtual void MoveTo(Vector3 target)
    {
        ulm.Initialize(transform.position, target, (target - transform.position).magnitude / speed);
    }
}
