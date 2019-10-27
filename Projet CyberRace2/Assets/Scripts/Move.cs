using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public Slider ThrustSlider;
    public int TrhrustUp=100;
    public float speed = 0.1f;
    float Roll = 3;
    float myDrag=0;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        myDrag = speed * 30;
        ThrustSlider.value = TrhrustUp;
        if(TrhrustUp<100)
        {
            TrhrustUp += 1;
            if(this.transform.position.y<2)
            {
                TrhrustUp += 4;
            }
        }
        if(TrhrustUp>100)
        {
            TrhrustUp = 100;
        }
        this.transform.position += this.transform.forward*speed;
       
       //Roll
       if(this.transform.position.y>3)//a remplacer par un raycast qui vois la fistance du sol
        {
            if (this.transform.rotation.eulerAngles.z < 90 && this.transform.rotation.eulerAngles.z > 20)//Side Q
            {
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y - (this.transform.rotation.eulerAngles.z/Roll) * Time.deltaTime, this.transform.rotation.eulerAngles.z);
            }
            if (this.transform.rotation.eulerAngles.z > 270 && this.transform.rotation.eulerAngles.z < 340)//Side E
            {
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + (Mathf.Abs((this.transform.rotation.eulerAngles.z-360))/Roll) * Time.deltaTime, this.transform.rotation.eulerAngles.z);
            }

            if (this.transform.rotation.eulerAngles.z < 170 && this.transform.rotation.eulerAngles.z > 90)//Side Q Down
            {
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y - 40 * Time.deltaTime, this.transform.rotation.eulerAngles.z);
                this.myDrag = speed*30 - (this.transform.rotation.eulerAngles.z / 6);
                print(this.transform.rotation.eulerAngles.z / 6);
            }
            if (this.transform.rotation.eulerAngles.z > 190 && this.transform.rotation.eulerAngles.z < 270)//Side E Down
            {
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 40 * Time.deltaTime, this.transform.rotation.eulerAngles.z);
                this.myDrag =speed*30 - (Mathf.Abs((this.transform.rotation.eulerAngles.z - 360)) / 6);


            }
            if(this.transform.rotation.eulerAngles.z < 270&& this.transform.rotation.eulerAngles.z>90)
            {
                this.rb.drag = 0;
            }
        }



        print(this.transform.rotation.eulerAngles.x);

        //Deplacements
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 15 * Time.deltaTime, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x - 15 * Time.deltaTime, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
            
           
           
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y+15*Time.deltaTime, this.transform.rotation.eulerAngles.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y - 15*Time.deltaTime, this.transform.rotation.eulerAngles.z);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z + 30 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z - 30 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (speed < 0.6f)
            {
                speed += 0.01f;
            }
            
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if(speed>0)
            {
                speed -= 0.01f;
            }
            
            
            
        }
        if (speed <= 0)
        {
            speed =0f;
        }


        if (Input.GetKey(KeyCode.R))//Stabilise
        {

            stablisising();
            
        }

        if (Input.GetKey(KeyCode.Space))//Lift up
        {

            this.transform.position += this.transform.up * 0.1f;
            TrhrustUp -= 5;

        }
        rb.drag = myDrag;
    }
    void stablisising()
    {
        //lerp d<une rotation vers (0,y,0);
        float y = this.transform.rotation.eulerAngles.y;
        float Startx = this.transform.rotation.eulerAngles.x%360f;
        float Startz = this.transform.rotation.eulerAngles.z%360f;
       
        if(Startx>180f&&Startx<360)
        {
            Startx += 0.1f;
            
        }
        if (Startx <180f&&Startx>0)
        {
            Startx -= 0.1f;
        }
        if (Startz > 180f && Startz < 360)
        {
            Startz += 0.1f;
        }
        if (Startz < 180f && Startz > 0)
        {
            Startz -= 0.1f;
        }
        this.transform.rotation = Quaternion.Euler(Startx, y, Startz);
        
        
    }
}
