using EditorExtend.GridEditor;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static readonly Dictionary<KeyCode, Vector3> DirectionDict = new()
    {
        { KeyCode.A,Vector3.left },
        { KeyCode.W,Vector3.up },
        { KeyCode.S,Vector3.down },
        { KeyCode.D,Vector3.right },
    };

    private static int Extend = 2;

    private IEventSystem eventSystem;
    public Transform follow;
    public bool isFollowing;
    public float moveSpeed;
    public Vector3 offset;

    public float xMin, yMin, xMax, yMax;

    public void ClampInMap()
    {
        float x = Mathf.Clamp(transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void BeforeDoAction(PawnEntity pawn)
    {
        if (!pawn.hidden)
        {
            follow = pawn.transform;
            isFollowing = true;
        }
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
    }

    private void Start()
    {
        IsometricGridManager igm = IsometricGridManager.Instance;
        xMin = xMax = transform.position.x;
        yMin = yMax = transform.position.y;
        foreach (GridObject obj in igm.ObjectDict.Values)
        {
            if(obj.IsGround)
            {
                Vector2 position = obj.transform.position;
                xMin = Mathf.Min(xMin, position.x);
                xMax = Mathf.Max(xMax, position.x);
                yMin = Mathf.Min(yMin, position.y);
                yMax = Mathf.Max(yMax, position.y);
            }
        }
        xMin -= Extend;
        yMin -= Extend;
        xMax += Extend;
        yMax += Extend;
    }

    private void OnEnable()
    {
        eventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.R))
        {
            isFollowing = true;
        }
        else
        {
            foreach(KeyCode keyCode in DirectionDict.Keys)
            {
                if (Input.GetKey(keyCode))
                {
                    isFollowing = false;
                    transform.position += moveSpeed * Time.deltaTime * DirectionDict[keyCode];
                }
            }
        }
        ClampInMap();
    }

    private void LateUpdate()
    {
        if (follow != null && isFollowing)
            transform.position = (follow.position + offset).ResetZ(-10);
    }
}
