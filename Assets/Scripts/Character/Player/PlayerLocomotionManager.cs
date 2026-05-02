using UnityEngine;

/// <summary>
/// 根据玩家输入和摄像机朝向处理玩家角色的移动与转向。
/// </summary>
public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private PlayerManager PlayerManager;

    [SerializeField] private float vertical_movement;
    [SerializeField] private float horizontal_movement;
    [SerializeField] private float move_amount;

    private Vector3 move_direction;
    private Vector3 target_rotation_direction;

    [SerializeField] private float walking_speed = 2.0f;
    [SerializeField] private float running_speed = 5.0f;
    [SerializeField] private float rotation_speed = 15.0f;

    protected override void Awake()
    {
        base.Awake();

        PlayerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        // 移动和旋转分开处理，便于后续插入锁定、翻滚或攻击状态。
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        vertical_movement = PlayerInputManager.Instance.GetVerticalInput();
        horizontal_movement = PlayerInputManager.Instance.GetHorizontalInput();
    }

    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();
        // 将输入转换到摄像机朝向，避免角色移动方向和视角脱节。
        move_direction = PlayerCamera.Instance.transform.forward * vertical_movement
                       + PlayerCamera.Instance.transform.right * horizontal_movement;
        move_direction.Normalize();
        move_direction.y = 0.0f;

        if (PlayerInputManager.Instance.GetMoveAmount() > 0.5f)
        {
            Debug.Log("Running");
            PlayerManager.CharacterController.Move(move_direction * running_speed * Time.deltaTime);
        }
        else if (PlayerInputManager.Instance.GetMoveAmount() <= 0.5f)
        {
            PlayerManager.CharacterController.Move(move_direction * walking_speed * Time.deltaTime);
        }
        
    }

    private void HandleRotation()
    {
        target_rotation_direction = Vector3.zero;
        // 角色朝向跟随摄像机空间中的输入方向，而不是世界坐标轴。
        target_rotation_direction = PlayerCamera.Instance.GetCameraObject().transform.forward * vertical_movement
                                  + PlayerCamera.Instance.GetCameraObject().transform.right * horizontal_movement;
        target_rotation_direction.Normalize();
        target_rotation_direction.y = 0.0f;

        if (target_rotation_direction == Vector3.zero)
        {
            target_rotation_direction = transform.forward;
        }

        Quaternion new_rotation = Quaternion.LookRotation(target_rotation_direction);
        Quaternion target_rotation = Quaternion.Slerp(transform.rotation, new_rotation, rotation_speed * Time.deltaTime);
        transform.rotation = target_rotation;
    }
}
