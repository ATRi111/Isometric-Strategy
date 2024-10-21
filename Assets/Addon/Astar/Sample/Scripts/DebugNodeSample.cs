using TMPro;
using UnityEngine;

namespace AStar.Sample
{
    public class DebugNodeSample : MonoBehaviour
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
        private Color color_route;
        [SerializeField]
        private Color color_blank;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            textbox = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Initialize(AStarNode node)
        {
            if(node.IsObstacle)
            {
                spriteRenderer.color = color_obstacle;
            }    
            else
            {
                spriteRenderer.color = node.state switch
                {
                    ENodeState.Open => color_open,
                    ENodeState.Close => color_close,
                    ENodeState.Route => color_route,
                    _ => color_blank,
                };
            }
            if (node.state == ENodeState.Blank || node.IsObstacle)
                textbox.text = string.Empty;
            else
                textbox.text = $"G:{(int)node.GCost}\n" +
                    $"H:{(int)node.HCost}\n" +
                    $"F:{(int)node.FCost}\n";
        }
    }
}