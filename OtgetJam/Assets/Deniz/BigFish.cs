using UnityEngine;

public class BigFish : Entity
{
    [SerializeField] int speed;


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.updateGravity();
        base.ApplyForceToTarget(MainCharacter.currentMainCharacterPosition, speed * Time.deltaTime);
    }
}
