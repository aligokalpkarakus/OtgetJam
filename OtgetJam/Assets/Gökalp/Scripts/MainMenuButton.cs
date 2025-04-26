using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu"; // Ana menü sahnenin adýný buraya yaz

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ReturnToMenuScene);
    }

    private void ReturnToMenuScene()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
