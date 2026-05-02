using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 读取玩家输入并将原始输入转换成移动系统使用的方向和移动强度。
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    [SerializeField] private Vector2 movement_input;
    [SerializeField] private float vertical_input;
    [SerializeField] private float horizontal_input;
    [SerializeField] private float move_amount;

    private PlayerControls PlayerControls;

    private void Awake()
    {
        // 输入管理器在菜单和世界场景之间共享，只保留第一个实例。
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;

        // 直接从 World Scene 启动时不会经过主菜单，用当前场景决定输入是否启用。
        if (SceneManager.GetActiveScene().buildIndex != WorldSaveGameManager.Instance.GetWorldSceneIndex())
        {
            Instance.enabled = false;
        }
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void OnSceneChanged(Scene OldScene, Scene NewScene)
    {
        if (NewScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
        {
            Instance.enabled = true;
        }
        else
        {
            Instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (PlayerControls == null)
        {
            PlayerControls = new PlayerControls();

            PlayerControls.PlayerMovement.Movement.performed += Context => movement_input = Context.ReadValue<Vector2>();
            // Input System 在释放按键或摇杆回中时触发 canceled，需要清空移动输入。
            PlayerControls.PlayerMovement.Movement.canceled += Context => movement_input = Vector2.zero;
        }
        PlayerControls.Enable();
    }

    private void HandleMovementInput()
    {
        vertical_input = movement_input.y;
        horizontal_input = movement_input.x;

        // 将二维输入压缩成移动强度，后续用于区分行走和跑步。
        move_amount = Mathf.Clamp01(Mathf.Abs(horizontal_input) + Mathf.Abs(vertical_input));

        if (move_amount > 0.0f && move_amount < 0.5f)
        {
            move_amount = 0.5f;
        }
        else if (move_amount > 0.5f && move_amount < 1.0f)
        {
            move_amount = 1.0f;
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled && PlayerControls != null)
        {
            if (focus)
            {
                PlayerControls.Enable();

            }
            else
            {
                PlayerControls.Disable();
            }
        }
    }

    public float GetMoveAmount()
    {
        return move_amount;
    }

    public float GetVerticalInput()
    {
        return vertical_input;
    }

    public float GetHorizontalInput()
    {
        return horizontal_input;
    }
}
