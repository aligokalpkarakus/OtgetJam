using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            ReloadCurrentScene();
        }
    }

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
