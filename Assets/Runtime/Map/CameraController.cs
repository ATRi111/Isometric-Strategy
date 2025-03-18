using EditorExtend.GridEditor;
using Services;
using Services.Event;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private readonly static int Extend = 2;

    private float defaultSize;
    public float CameraSize
    {
        get => myCamera.orthographicSize;
        private set => myCamera.orthographicSize = value;
    }

    private IEventSystem eventSystem;
    private Camera myCamera;

    public Transform follow;
    public bool isFollowing;
    public float moveSpeed;
    public Vector3 offset;

    public float zoomSpeed;
    public float maxSize;
    public float minSize;
    public Transform cameraFrame;

    private float xMin, yMin, xMax, yMax;

    private float zoomRatio;
    private Vector3 moveDirection;

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

    public void Move()
    {
        transform.position += CameraSize / defaultSize * moveSpeed * Time.fixedDeltaTime * moveDirection;
        moveDirection = Vector3.zero;
    }

    public void Zoom()
    {
        zoomRatio = Mathf.Clamp(zoomRatio, minSize / CameraSize, maxSize / CameraSize);
        cameraFrame.transform.localScale *= zoomRatio;
        CameraSize *= zoomRatio;
        zoomRatio = 1f;
    }

    public void Follow()
    {
        if (follow != null && isFollowing)
            transform.position = (follow.position + offset).ResetZ(-10);
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        myCamera = GetComponent<Camera>();
        defaultSize = CameraSize;
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
        zoomRatio = 1f;
        moveDirection = Vector2.zero;
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void Update()
    {
        float delta = Input.GetAxisRaw("Mouse ScrollWheel");
        zoomRatio *= (1f - zoomSpeed * delta);

        if (Input.GetKeyUp(KeyCode.R))
        {
            isFollowing = true;
            zoomRatio = defaultSize / CameraSize;
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            moveDirection = new Vector3(x, y, 0).normalized;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Zoom();
        Follow();
        ClampInMap();
    }
}
