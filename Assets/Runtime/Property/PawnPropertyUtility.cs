using Services;
using Services.Asset;
using UnityEngine;

public class PawnPropertyUtility : MonoBehaviour
{
    private static readonly PawnPropertyDict PropertyDict;

    public FindPawnPropertySO GetProperty(string propertyName)
    {
        return PropertyDict[propertyName];
    }

    static PawnPropertyUtility()
    {
        PropertyDict = ServiceLocator.Get<IAssetLoader>().Load<PawnPropertyDict>("´ÊÌõ×Öµä");
    }
}
