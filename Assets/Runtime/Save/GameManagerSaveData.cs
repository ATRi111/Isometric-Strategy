using Services.Save;
using UnityEngine;

[System.Serializable]
public class GameManagerSaveData : SaveData
{
    private GameManager manager;
    public int battleSceneIndex;

    public override void Load()
    {
        manager.battleSceneIndex = battleSceneIndex;
    }

    public override void Save()
    {
        battleSceneIndex = manager.battleSceneIndex;
    }

    public override void Initialize(string identifier, Object obj)
    {
        base.Initialize(identifier, obj);
        manager = (obj as GameObject).GetComponent<GameManager>();
    }
}
