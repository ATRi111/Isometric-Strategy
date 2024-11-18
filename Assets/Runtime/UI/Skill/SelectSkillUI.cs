using UIExtend;
using UnityEngine;

public class SelectSkillUI : MonoBehaviour
{
    private SkillUIManager skillUIManager;
    [SerializeField]
    private int maxIconCount;
    [SerializeField]
    private GameObject prefab;
    private SkillIcon[] icons;
    private RectTransform rectTransform;

    private void SelectSkill(PawnBrain brain)
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

    private void AfterSelectSkill(Skill _)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].canvasGrounp.Visible = false;
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        skillUIManager = SkillUIManager.FindInstance();
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
        skillUIManager.SelectSkill += SelectSkill;
        skillUIManager.AfterSelectSkill += AfterSelectSkill;
    }

    private void OnDisable()
    {
        skillUIManager.SelectSkill += SelectSkill;
        skillUIManager.AfterSelectSkill -= AfterSelectSkill;
    }
}
