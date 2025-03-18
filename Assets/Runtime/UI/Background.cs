using UnityEngine;

public class Background : MonoBehaviour
{
    public float depth = 5f;

    private Camera levelCamera;
    private Vector3 selfPosition;
    private Vector3 cameraPosition;

    private void Start()
    {
        levelCamera = GameObject.Find("LevelCamera").GetComponent<Camera>();
        selfPosition = transform.position;
        cameraPosition = levelCamera.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = selfPosition + depth / (depth - 1f) * (levelCamera.transform.position - cameraPosition);
    }
}
