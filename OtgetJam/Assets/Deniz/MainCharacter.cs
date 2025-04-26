using UnityEngine;

public class MainCharacter : Entity
{
    [SerializeField] float speed;
    [SerializeField] float dash_speed;
    [SerializeField] float ruzgar_speed;
    [SerializeField] float coolDownPeriod;
    

    public static Vector2 currentMainCharacterPosition;
    private float nextAvaliableTimeDash = 0f;

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
                
            }
            if (Input.GetKey(KeyCode.S))
            {
                move_v2 += Vector2.down;
                
            }
            if (Input.GetKey(KeyCode.D))
            {
                move_v2 += Vector2.right;
                
            }
            if (Input.GetKey(KeyCode.A))
            {
                move_v2 += Vector2.left;
               
            }

            // normalize ediyoz ki çarpazda hýzlý gitmesin
            if (move_v2 != Vector2.zero)
                move_v2 = move_v2.normalized;

            if (Input.GetKey(KeyCode.LeftShift) && Time.time >= nextAvaliableTimeDash)
                dash(move_v2);
            else
                base.moveVectorized(move_v2, this.speed * Time.deltaTime);

        }
        else if (currentLayer == "Ruzgar")
        {
            Debug.Log("RUZGAR");
            base.moveImpulse(Vector2.right, this.ruzgar_speed*Time.deltaTime);
        }

    }

   

    private void dash(Vector2 dir){
        base.moveImpulse(dir, this.dash_speed);
        nextAvaliableTimeDash = Time.time + coolDownPeriod;
        Debug.Log("dashed");
    }


    
}
