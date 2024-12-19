using TMPro;
using UnityEngine;

public class DebugPlanUI : MonoBehaviour
{
    public Plan plan;

    private SpriteRenderer spriteRenderer;
    private Canvas canvas;
    private TextMeshProUGUI textbox;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>();
        textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetPlan(Plan plan)
    {
        string name = plan.action.skill.name;
        int hash = name.GetHashCode();
        float r = (hash & 0xFF) / 255f; 
        float g = ((hash >> 8) & 0xFF) / 255f; 
        float b = ((hash >> 16) & 0xFF) / 255f;
        spriteRenderer.color = new Color(r, g, b, 0.6f);
        this.plan = plan;
        textbox.text = 
            $"V:{plan.value:F2}\n" +
            $"{plan.action.skill.name}\n" +
            $"T:{plan.action.Time}\n";
    }

    private void Update()
    {
        canvas.sortingOrder = spriteRenderer.sortingOrder;
    }
}
