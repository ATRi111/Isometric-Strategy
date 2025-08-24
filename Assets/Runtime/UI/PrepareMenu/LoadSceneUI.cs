using MyTimer;
using MyTool;
using Services;
using Services.Event;
using Services.SceneManagement;
using TMPro;
using UIExtend;
using UnityEngine;

public class LoadSceneUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    private CanvasGroupPlus canvasGroup;
    private TextMeshProUGUI tmp;
    private Material textMaterial;
    private TimerOnly textAnimationTimer;
    [SerializeField]
    private float animationDuration;

    public SerializedDictionary<string, string> levelNameDict;

    private void AfterMapInitialize()
    {
        textAnimationTimer.Restart();
    }

    private void OnTick(float t)
    {
        float offset = Mathf.Lerp(-1, 1, t / animationDuration);
        textMaterial.SetFloat("_GlowOffset", offset);
    }

    private void AfterComplete(float _)
    {
        textMaterial.SetFloat("_GlowOffset", -1);
        canvasGroup.Visible = false;
    }

    private void BeforeLoadScene(int index)
    {
        string sceneName = SceneControllerUtility.ToSceneName(index);
        if (!levelNameDict.TryGetValue(sceneName, out string levelName))
        {
            levelName = sceneName;
        }
        tmp.text = levelName;
        textMaterial.SetFloat("_GlowOffset", -1);
        canvasGroup.Visible = true;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        textMaterial = tmp.materialForRendering;
        levelNameDict.Refresh();
        textAnimationTimer = new TimerOnly();
        textAnimationTimer.Initialize(animationDuration, false);
        textAnimationTimer.AfterComplete += AfterComplete;
        textAnimationTimer.OnTick += OnTick;
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterMapInitialize, AfterMapInitialize);
        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
        BeforeLoadScene(ServiceLocator.Get<GameManager>().battleSceneIndex);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.AfterMapInitialize, AfterMapInitialize);
        eventSystem.RemoveListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
    }
}
