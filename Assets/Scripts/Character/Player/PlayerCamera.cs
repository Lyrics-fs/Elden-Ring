using UnityEngine;

/// <summary>
/// 玩家摄像机入口，提供跨场景访问主摄像机的引用。
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }

    [SerializeField] private Camera CameraObject;

    private void Awake()
    {
        // 摄像机随玩家系统跨场景保留，只允许存在一个活动实例。
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (CameraObject == null)
        {
            CameraObject = GetComponentInChildren<Camera>();
        }
    }

    public Camera GetCameraObject()
    {
        return CameraObject;
    }
}
