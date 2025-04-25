using UnityEngine;

public class MainCharacter : Entity
{
    [SerializeField] int speed;

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 move_v2 = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            move_v2 = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move_v2 = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move_v2 = Vector2.right;
        }
        base.moveVectorized(move_v2, speed);

    }
}
