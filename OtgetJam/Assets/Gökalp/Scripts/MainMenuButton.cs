using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu"; // Ana men� sahnenin ad�n� buraya yaz

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ReturnToMenuScene);
    }

    private void ReturnToMenuScene()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
