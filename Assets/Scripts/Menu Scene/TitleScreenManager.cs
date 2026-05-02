using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 标题界面按钮事件入口，负责启动网络会话和进入新游戏。
/// </summary>
public class TitleScreenManager : MonoBehaviour
{
    /// <summary>
    /// 作为主机启动 Netcode 会话，供菜单按钮调用。
    /// </summary>
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    /// <summary>
    /// 通过世界管理器进入新游戏流程，供菜单按钮调用。
    /// </summary>
    public void StartNewGame()
    {
        StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
    }
}
