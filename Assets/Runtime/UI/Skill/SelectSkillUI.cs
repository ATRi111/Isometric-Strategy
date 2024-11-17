using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class SelectSkillUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    [SerializeField]
    private int maxIconCount;
    [SerializeField]
    private GameObject prefab;
    private SkillIcon[] icons;
    private RectTransform rectTransform;

    private void OnHumanControl(PawnBrain brain)
    {
        transform.position = Camera.main.WorldToScreenPoint(brain.transform.position);
        int count = 0;
        foreach(Skill skill in brain.learnedSkills)
        {
            icons[count].canvasGrounp.Visible = true;
            icons[count].SetSkill(skill);
            count++;
        }
        for (; count < icons.Length; count++)
        {
            icons[count].canvasGrounp.Visible = false;
        }
        UIExtendUtility.ClampInScreen(rectTransform);
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        rectTransform = GetComponent<RectTransform>();
        icons = new SkillIcon[maxIconCount];
        for (int i = 0; i < maxIconCount; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            icons[i] = obj.GetComponent<SkillIcon>();
            icons[i].transform.localPosition = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        eventSystem.AddListener<PawnBrain>(EEvent.OnHumanControl, OnHumanControl);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnBrain>(EEvent.OnHumanControl, OnHumanControl);
    }
}
