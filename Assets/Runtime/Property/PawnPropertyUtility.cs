using Services;
using Services.Asset;

public static class PawnPropertyUtility
{
    private static readonly PawnPropertyDict PropertyDict;

    public static FindPawnPropertySO GetProperty(string propertyName)
    {
        return PropertyDict[propertyName];
    }

    static PawnPropertyUtility()
    {
        PropertyDict = ServiceLocator.Get<IAssetLoader>().Load<PawnPropertyDict>("´ÊÌõ×Öµä");
        PropertyDict.Initialize();
    }
}
