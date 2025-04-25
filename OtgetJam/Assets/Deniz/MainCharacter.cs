using UnityEngine;

public class MainCharacter : Entity
{
    [SerializeField] int speed;

    private string currentLayer = "";

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 move_v2 = Vector2.zero;
        currentLayer = base.checkCurrentLayer(); // þu an bulunan layer
        updateGravity();//suda ve havada olma durumuna göre gravity scaleýný günceller.


        if (currentLayer == "Water")
        {
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

    private void updateGravity()
    {
        string layerName = base.checkCurrentLayer();

        if (layerName == "Water")//Water ise gravity 0 olcak 
        {
            base.setGravity(0);
            base.setDrag(2);
        }
            
        else if (layerName == "Air")
        {
            base.setGravity(1);
            base.setDrag(1);
        }
            
        
    }

    
}
