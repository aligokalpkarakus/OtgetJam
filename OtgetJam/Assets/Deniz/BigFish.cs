using UnityEngine;

public class BigFish : Entity
{
    [SerializeField] int speed;


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        base.updateGravity();
        base.ApplyForceToTarget(MainCharacter.currentMainCharacterPosition, speed * Time.deltaTime);
    }
}
