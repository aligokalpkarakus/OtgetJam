using UnityEngine;

public class MainCharacter : Entity
{
    [SerializeField] int  speed;
    [SerializeField] int dash_speed;

    public static Vector2 currentMainCharacterPosition;

    private string currentLayer = "";

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        currentMainCharacterPosition = base.rb.position;


        Vector2 move_v2 = Vector2.zero;
        currentLayer = base.checkCurrentLayer(); // þu an bulunan layer
        base.updateGravity();//suda ve havada olma durumuna göre gravity scaleýný günceller.


        if (currentLayer == "Water")
        {

            if (Input.GetKey(KeyCode.W))
            {
                move_v2 += Vector2.up;
                Debug.Log("up");
            }
            if (Input.GetKey(KeyCode.S))
            {
                move_v2 += Vector2.down;
                Debug.Log("down");
            }
            if (Input.GetKey(KeyCode.D))
            {
                move_v2 += Vector2.right;
                Debug.Log("right");
            }
            if (Input.GetKey(KeyCode.A))
            {
                move_v2 += Vector2.left;
                Debug.Log("left");
            }

            // normalize ediyoz ki çarpazda hýzlý gitmesin
            if (move_v2 != Vector2.zero)
                move_v2 = move_v2.normalized;

            if (Input.GetKey(KeyCode.LeftShift))
                dash(move_v2);
            else
                base.moveVectorized(move_v2, this.speed);

        }



    }

    

    private void dash(Vector2 dir){
        base.moveImpulse(dir, this.dash_speed);
        Debug.Log("dashed");
    }


    
}
