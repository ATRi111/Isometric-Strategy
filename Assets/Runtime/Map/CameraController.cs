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

    private IEventSystem eventSystem;
    public Transform follow;
    public bool isFollowing;
    public float moveSpeed;
    public Vector3 offset;

    private void BeforeDoAction(PawnEntity pawn)
    {
        follow = pawn.transform;
        isFollowing = true;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
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
    }

    private void LateUpdate()
    {
        if(follow != null && isFollowing)
            transform.position = follow.position + offset;
    }
}
