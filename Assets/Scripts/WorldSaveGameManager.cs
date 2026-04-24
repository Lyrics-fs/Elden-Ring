using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WorldSaveGameManager : MonoBehaviour
{
    [SerializeField] private int world_scene_index = 1;

    public static WorldSaveGameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame()
    {
        AsyncOperation _LoadOperation = SceneManager.LoadSceneAsync(world_scene_index);
        yield return null;
    }
}
