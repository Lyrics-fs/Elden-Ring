using UnityEngine;

/// <summary>
/// 玩家角色入口，只在本机拥有者上驱动输入和移动。
/// </summary>
public class PlayerManager : CharacterManager
{
    private PlayerLocomotionManager PlayerLocomotionManager;

    protected override void Awake()
    {
        base.Awake();

        PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();

        // 非拥有者只接收网络同步，不执行本地输入驱动的移动逻辑。
        if (!IsOwner)
            return;

        PlayerLocomotionManager.HandleAllMovement();
    }
}
