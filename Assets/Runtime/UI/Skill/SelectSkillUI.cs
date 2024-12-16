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

    private PawnEntity current;

    private void SelectSkill(PawnEntity pawn)
    {
        current = pawn;
        int count = 0;
        foreach(Skill skill in pawn.Brain.learnedSkills)
        {
            if (skill.CanUse(pawn, pawn.Igm))
            {
                icons[count].canvasGroup.Visible = true;
                icons[count].SetSkill(skill);
                count++;
            }
        }
        for (; count < icons.Length; count++)
        {
            icons[count].canvasGroup.Visible = false;
        }
        UIExtendUtility.ClampInScreen(rectTransform);
    }

    private void AfterSelectSkill(Skill _)
    {
        current = null;
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].canvasGroup.Visible = false;
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

    private void Update()
    {
        if (current != null)
            transform.position = Camera.main.WorldToScreenPoint(current.transform.position);
    }
}
