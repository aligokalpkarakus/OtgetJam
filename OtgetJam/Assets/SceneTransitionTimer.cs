using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionTimer : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private float waitTime = 5f;

    private void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        // Belirlenen s�re kadar bekle
        yield return new WaitForSeconds(waitTime);

        // Bir sonraki sahneye ge�
        SceneManager.LoadScene(nextSceneName);
    }
}