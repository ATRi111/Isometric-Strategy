using EditorExtend.GridEditor;
using Services;
using Services.Event;
using System;
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

    public Action AfterFixedUpdate;

    private IEventSystem eventSystem;
    private Camera myCamera;
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    [SerializeField] 
    private float moveSpeed;
    public Vector3 offset;

    public float zoomSpeed;
    [SerializeField]
    private float maxSize;
    [SerializeField]
    private float minSize;

    private float xMin, yMin, xMax, yMax;
    private float aspectRatio;

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
            transform.position = pawn.transform.position.ResetZ(transform.position.z);
            zoomRatio = defaultSize / CameraSize;
        }
    }

    private void OnDoAction(PawnAction action)
    {
        float x0, x1, y0, y1;
        x0 = y0 = float.MaxValue;
        x1 = y1 = float.MinValue;
        void UpdateRect(Vector3 position)
        {
            x0 = Mathf.Min(x0, position.x);
            x1 = Mathf.Max(x1, position.x);
            y0 = Mathf.Min(y0, position.y);
            y1 = Mathf.Max(y1, position.y);
        }

        UpdateRect(action.agent.transform.position);
        for (int i = 0; i < action.area.Count; i++)
        {
            UpdateRect(Igm.CellToWorld(action.area[i]));
        }

        float size = 0.55f * Mathf.Max(y1 - y0, (x1 - x0) / aspectRatio);
        zoomRatio = size / CameraSize;
        transform.position = new((x0 + x1) / 2, (y0 + y1) / 2, transform.position.z);
    }

    private void Move()
    {
        transform.position += CameraSize / defaultSize * moveSpeed * Time.fixedDeltaTime * moveDirection;
        moveDirection = Vector3.zero;
    }

    private void Zoom()
    {
        zoomRatio = Mathf.Clamp(zoomRatio, minSize / CameraSize, maxSize / CameraSize);
        CameraSize *= zoomRatio;
        zoomRatio = 1f;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        myCamera = GetComponent<Camera>();
        defaultSize = CameraSize;
        aspectRatio = Screen.width / Screen.height;
    }

    private void Start()
    {
        IsometricGridManager igm = IsometricGridManager.Instance;
        xMin = xMax = transform.position.x;
        yMin = yMax = transform.position.y;
        foreach (GridObject obj in igm.ObjectDict.Values)
        {
            if (obj.IsGround)
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
        eventSystem.AddListener<PawnAction>(EEvent.OnDoAction, OnDoAction);
        zoomRatio = 1f;
        moveDirection = Vector2.zero;
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
        eventSystem.RemoveListener<PawnAction>(EEvent.OnDoAction, OnDoAction);
    }

    private void Update()
    {
        float delta = Input.GetAxisRaw("Mouse ScrollWheel");
        zoomRatio *= (1f - zoomSpeed * delta);

        if (Input.GetKeyUp(KeyCode.R))
        {
            zoomRatio = defaultSize / CameraSize;
        }
        else
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(x, y, 0).normalized;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Zoom();
        ClampInMap();
        AfterFixedUpdate?.Invoke();
    }
}
