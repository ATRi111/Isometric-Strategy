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
    private float maxSize;
    [SerializeField]
    private float minSize;

    [SerializeField]
    [Range(5f, 50f)]
    private float moveSpeed;    //1秒内可移动的距离
    public float AdjustedMoveSpeed
    {
        get
        {
            float d0 = (transform.position - prevPosition).magnitude;
            float d1 = (transform.position - targetPosition).magnitude;
            float t = d0 / Mathf.Max(1, d0 + d1);
            float k = Mathf.Clamp((targetPosition - prevPosition).magnitude / moveSpeed, 0.2f, 2f); //根据路程调整速度
            t = Mathf.Sin(t * Mathf.PI);
            return k * Mathf.Lerp(moveSpeed / 2, moveSpeed * 2, t); //根据到起点/终点的距离平滑移动
        }
    }
    [SerializeField]
    [Range(5f, 50f)]
    private float zoomSpeed;    //1秒内可放大/缩小的倍数
    public float AdjustedZoomSpeed
    {
        get
        {
            float d0 = Mathf.Abs(CameraSize - prevSize);
            float d1 = Mathf.Abs(CameraSize - targetSize);
            float t = d0 / Mathf.Max(1, d0 + d1);
            t = Mathf.Sin(t * Mathf.PI);
            return Mathf.Lerp(zoomSpeed / 2, zoomSpeed * 2, t); //根据起始尺寸/最终尺寸平滑缩放
        }
    }


    private float xMin, yMin, xMax, yMax;
    private float aspectRatio;

    private float prevSize;
    private float targetSize;
    private Vector3 prevPosition;
    private Vector3 targetPosition;

    private Transform follow;

    public void ClampInMap()
    {
        float x = Mathf.Clamp(targetPosition.x, xMin, xMax);
        float y = Mathf.Clamp(targetPosition.y, yMin, yMax);
        targetPosition = new Vector3(x, y, targetPosition.z);
    }

    private void BeforeDoAction(PawnEntity pawn)
    {
        if (!pawn.hidden)
        {
            targetPosition = pawn.transform.position.ResetZ(transform.position.z);
            follow = pawn.transform;
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
        x0 -= Extend;
        y0 -= Extend;
        x1 += Extend;
        y1 += Extend;

        float size = 0.5f * Mathf.Max(y1 - y0, (x1 - x0) / aspectRatio);
        targetSize = size;
        targetPosition = new((x0 + x1) / 2, (y0 + y1) / 2, transform.position.z);
    }

    private void Move()
    {
        Vector3 delta = targetPosition - transform.position;
        if (delta.magnitude < AdjustedMoveSpeed * Time.fixedDeltaTime)
        {
            transform.position = targetPosition;
            prevPosition = targetPosition;
        }
        else
        {
            delta = AdjustedMoveSpeed * Time.fixedDeltaTime * delta.normalized;
            transform.position += delta;
        }
    }

    private void Zoom()
    {
        targetSize = Mathf.Clamp(targetSize, minSize, maxSize);
        float delta = targetSize > CameraSize ? targetSize / CameraSize : CameraSize / targetSize;
        float zoomRatio = Mathf.Pow(AdjustedZoomSpeed, Time.fixedDeltaTime);
        if (delta < zoomRatio)
        {
            CameraSize = targetSize;
            prevSize = targetSize;
        }
        else
        {
            CameraSize = targetSize > CameraSize ? CameraSize * zoomRatio : CameraSize / zoomRatio;
        }
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
        prevPosition = targetPosition = transform.position;
        prevSize = targetSize = CameraSize;
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
        eventSystem.RemoveListener<PawnAction>(EEvent.OnDoAction, OnDoAction);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (follow != null)
                transform.position = follow.transform.position;
            targetPosition = transform.position;
            CameraSize = defaultSize;
            targetSize = defaultSize;
        }
        else
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Vector3 moveDirection = new Vector3(x, y, 0).normalized;
            targetPosition += 0.5f * moveSpeed * Time.deltaTime * moveDirection;

            float delta = Input.GetAxisRaw("Mouse ScrollWheel");
            float zoomRatio = Mathf.Pow(zoomSpeed, Time.deltaTime);
            if (delta > 0)
                targetSize /= zoomRatio;
            else if (delta < 0)
                targetSize *= zoomRatio;
        }
    }

    private void FixedUpdate()
    {
        ClampInMap();
        Move();
        Zoom();
        AfterFixedUpdate?.Invoke();
    }
}
