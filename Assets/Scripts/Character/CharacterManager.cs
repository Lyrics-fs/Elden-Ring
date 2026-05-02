using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 所有联网角色的基础管理器，负责持久化角色对象并同步基础网络状态。
/// </summary>
public class CharacterManager : NetworkBehaviour
{
    public CharacterController CharacterController { get; private set; }
    
    public CharacterNetworkManager CharacterNetworkManager { get; private set; }

    protected virtual void Awake()
    {
        // 角色由网络系统生成后需要跨场景保留，避免加载世界场景时被销毁。
        DontDestroyOnLoad(this);

        CharacterController = GetComponent<CharacterController>();
        CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        if(IsOwner)
        {
            // 当前由拥有者写入位置和旋转，远端实例只读取并插值显示。
            CharacterNetworkManager.SetNetworkPosition(transform.position);
            CharacterNetworkManager.SetNetworkRotation(transform.rotation);
        }
        else
        {
            transform.position = CharacterNetworkManager.SmoothNetworkPosition(transform.position);

            transform.rotation = Quaternion.Slerp
                (transform.rotation, 
                 CharacterNetworkManager.GetNetworkRotation(), 
                 CharacterNetworkManager.GetNetworkRotationSmoothTime());
        }
    }
}
