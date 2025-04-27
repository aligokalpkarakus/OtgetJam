using UnityEngine;

public class AutoNetDropper : MonoBehaviour
{
    public GameObject netPrefab;
    public Transform dropPoint;
    public float dropInterval = 6f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= dropInterval)
        {
            DropNet();
            timer = 0f;
        }
    }

    void DropNet()
    {
        SoundManager.Instance.PlaySound(2);
        Instantiate(netPrefab, dropPoint.position, Quaternion.identity);
    }
}
