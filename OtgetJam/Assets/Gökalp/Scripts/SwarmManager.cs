using UnityEngine;
using System.Collections.Generic;

public class SwarmManager : MonoBehaviour
{
    [SerializeField] private List<SwarmFish> swarmFishList = new List<SwarmFish>();
    [SerializeField] private float minDashInterval = 2f;
    [SerializeField] private float maxDashInterval = 5f;

    private float nextDashTime = 0f;

    private void Start()
    {
        ScheduleNextDash();
    }

    private void Update()
    {
        if (Time.time >= nextDashTime)
        {
            TriggerRandomDash();
            ScheduleNextDash();
        }
    }

    private void ScheduleNextDash()
    {
        nextDashTime = Time.time + Random.Range(minDashInterval, maxDashInterval);
    }

    private void TriggerRandomDash()
    {
        if (swarmFishList.Count == 0) return;

        int randomIndex = Random.Range(0, swarmFishList.Count);
        swarmFishList[randomIndex].PrepareToDash();
    }
}
