using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 玩家 UI 入口，目前承载网络连接相关的调试开关。
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; private set; }

    [Header("NETWORK JOIN")]
    [SerializeField] private bool start_game_client;

    private void Awake()
    {
        // UI 管理器跨场景保留，避免切换到世界场景后丢失连接控制入口。
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (start_game_client)
        {
            // Inspector 调试开关：先关闭当前网络会话，再作为客户端重新连接。
            start_game_client = false;
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }
}
