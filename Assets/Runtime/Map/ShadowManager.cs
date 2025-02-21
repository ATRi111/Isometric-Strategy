using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    private IsometricGridManager igm;
    private Dictionary<ShadowVertex, float> visibilityCache = new();

    public float GetVisibility(ShadowVertex vertex)
    {
        if (!visibilityCache.ContainsKey(vertex))
            visibilityCache.Add(vertex, CalculateVisibility(vertex));
        return visibilityCache[vertex];
    }

    private float CalculateVisibility(ShadowVertex vertex)
    {
        return 1f;
    }

    private void Awake()
    {
        igm = GetComponent<IsometricGridManager>();
    }
}
