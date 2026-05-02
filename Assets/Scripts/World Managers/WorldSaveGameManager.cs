using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 管理世界场景加载流程，后续可扩展为存档创建、读取和场景恢复入口。
/// </summary>
public class WorldSaveGameManager : MonoBehaviour
{
    [SerializeField] private int world_scene_index = 1;

    public static WorldSaveGameManager Instance { get; private set; }

    private void Awake()
    {
        // 存档和场景流程管理器需要跨场景存在，重复实例直接销毁。
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame()
    {
        // 使用 Build Settings 中配置的场景索引加载世界场景。
        SceneManager.LoadSceneAsync(world_scene_index);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return world_scene_index;
    }
}
