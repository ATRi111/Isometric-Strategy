using AStar;
using TMPro;
using UnityEngine;

public class ANodeDebugUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI textbox;

    [SerializeField]
    private Color color_open;
    [SerializeField]
    private Color color_close;
    [SerializeField]
    private Color color_obstacle;
    [SerializeField]
    private Color color_output;
    [SerializeField]
    private Color color_available;
    [SerializeField]
    private Color color_blank;
    [SerializeField]
    private Color color_to;
    [SerializeField]
    private Vector2Int position;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(AStarNode node)
    {
        position = node.Position;
        if (node.IsObstacle)
        {
            spriteRenderer.color = color_obstacle;
        }
        else if (node.process.output.Contains(node))
        {
            spriteRenderer.color = color_output;
        }
        else if (node == node.process.To)
        {
            spriteRenderer.color = color_to;
        }
        else if (node.process.available.Contains(node))
        {
            spriteRenderer.color = color_available;
        }
        else
        {
            spriteRenderer.color = node.state switch
            {
                ENodeState.Open => color_open,
                ENodeState.Close => color_close,
                _ => color_blank,
            };
        }
        if (node.state == ENodeState.Blank || node.IsObstacle)
            textbox.text = string.Empty;
        else
            textbox.text = $"G:{Mathf.RoundToInt(10 * node.GCost)}\n" +
                $"H:{Mathf.RoundToInt(10 * node.HCost)}\n" +
                $"F:{Mathf.RoundToInt(10 * node.WeightedFCost)}\n";
    }
}
