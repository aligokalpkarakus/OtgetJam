using UnityEngine;

public class AutoHarpoonShooter : MonoBehaviour
{
    public GameObject harpoonPrefab;
    public Transform firePoint;
    public float shootInterval = 4f;
    private float timer;
    private Transform fishTarget;

    void Start()
    {
        fishTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            ShootHarpoon();
            timer = 0f;
        }
    }

    void ShootHarpoon()
    {
        if (fishTarget == null) return;

        SoundManager.Instance.PlaySound(2);

        Vector2 direction = (fishTarget.position - firePoint.position).normalized;

        GameObject harpoon = Instantiate(harpoonPrefab, firePoint.position, Quaternion.identity);
        harpoon.GetComponent<HarpoonBehaviour>().SetDirection(direction);
    }

}
