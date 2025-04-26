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
        // Belirlenen süre kadar bekle
        yield return new WaitForSeconds(waitTime);

        // Bir sonraki sahneye geç
        SceneManager.LoadScene(nextSceneName);
    }
}