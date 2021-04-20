using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{ 

    public Light valo;

    PlayerInputActions pia;
    CharacterController player;

    public float normalSpeed = 10;
    public float fastSpeed = 30;
    float currentSpeed = 10;

    Vector3 gVelocity;
    public float gravity = -10f;
    public Transform bottom;
    public LayerMask groundMask;
    bool grounded;

    Transform pickupItem;

    // Start is called before the first frame update
    void Start()
    {
        gVelocity = Vector3.zero;
        pia = new PlayerInputActions();
        pia.Enable();

        pia.Land.Pick.started += (x) => Pickup();
        pia.Land.Pick.canceled += (x) => Drop();



     
        player = GetComponent<CharacterController>();

        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ControlMove();
        ControlGravity();
        ControlJump();

     
        /*Koodi oven avaukseen raycastilla (katsesuunta)
         
        RaycastHit rHit;
        bool collide = Physics.Raycast(transform.position, transform.forward, out rHit, 5);

        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);

        if(collide && rHit.collider.tag == "Door")
        {
            rHit.collider.GetComponent<Animator>().SetTrigger("OpenDoor");
        }*/
        
    }

    void Pickup()
    {
        Transform cam = Camera.main.transform;

        RaycastHit hit;

        //L‰hetet‰‰n laatikkomallinen "s‰de" kameran keskelt‰
        //et‰isyydelle 3. Regoidaan vain collidereihin, joilla Layer on Pickable
        bool isHit = Physics.BoxCast(cam.position, new Vector3(1, 1, 1), 
            cam.forward, out hit , cam.rotation, 3, LayerMask.GetMask("Pickable"));

        if(isHit)
        {
            pickupItem = hit.transform;
            pickupItem.GetComponent<Rigidbody>().isKinematic = true;
            pickupItem.parent = cam.transform;

            pickupItem.GetComponent<Renderer>().material.color = Color.yellow;
        }

    }

    void Drop()
    {
        if( pickupItem != null )
        {
            pickupItem.GetComponent<Rigidbody>().isKinematic = false;
            pickupItem.parent = null;

            pickupItem.GetComponent<Renderer>().material.color = new Color(58f/255f, 1, 0);

            pickupItem = null;
        }
    }


    void ControlJump()
    {
        if( pia.Land.Jump.triggered && grounded)
        {
            gVelocity.y = 5f;
        }
    }

    void ControlGravity()
    {
        grounded = Physics.CheckSphere(bottom.position, 0.4f, groundMask);

        if( grounded && gVelocity.y < 0)
        {
            gVelocity.y = -2f;
        }

        gVelocity.y += gravity * Time.deltaTime;

        player.Move(gVelocity * Time.deltaTime);

    }

    void ControlMove()
    {
        //Valitaan nopeus (shift-nappi)
        currentSpeed = pia.Land.Fast.ReadValue<float>() > 0 ? fastSpeed : normalSpeed;

        //Haetaan nuolin‰pp‰inten arvot vektorina
        Vector2 move = pia.Land.Move.ReadValue<Vector2>();

        //Liikutetaan nuolin‰pp‰inten mukaiseen suuntaan
        Vector3 moveDirecion = transform.forward * move.y + transform.right * move.x;
        player.Move(moveDirecion * Time.deltaTime * currentSpeed);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Box")
        {
         
            hit.gameObject.GetComponent<Animator>().Play("OpenBox");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AmmoBox")
        {
            //Pelaajan toisella scriptill‰ ShootControl on metodi MoreAmmo
            gameObject.SendMessage("MoreAmmo", 10);
        }
        else if( other.tag == "Door" )
        {
            other.GetComponent<Animator>().SetTrigger("OpenDoor");
        }
    }

}
