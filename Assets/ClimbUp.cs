using UnityEngine;
using System.Collections;

public class ClimbUp : MonoBehaviour {

    float speed = 5.0F;
    float rotationSpeed = 100.0F;
    float lerpSpeed = 5.0F;
    Animator anim;
    bool isHanging = false;
    Transform animRootTarget;  // anchor

    public void GrabEdge(Transform rootTarget)
    {
        if(isHanging) return;
        anim.SetTrigger("grabEdge");
        this.GetComponent<Rigidbody>().isKinematic = true; // leaves character in the system, but not affected by the physics system
        isHanging = true;
        animRootTarget = rootTarget; // anchor point (cube on side of container)
        Debug.Log(rootTarget.tag) ;
        Plane rootPlane = new Plane(
            animRootTarget.position,
            animRootTarget.position + animRootTarget.right,
            animRootTarget.position + animRootTarget.up);

        Vector3 adjustedPos = new Vector3(
            this.transform.position.x,  // bot's x position
            animRootTarget.position.y,  // cube's height position
            this.transform.position.z);  // bot's z position

        Ray ray = new Ray(adjustedPos, animRootTarget.forward);
        float rayDistance;
        if (rootPlane.Raycast(ray, out rayDistance))
        {
            animRootTarget.position = ray.GetPoint(rayDistance); // position where the two objects meet, new anchor point
        }
                
    }

    public void StandingUp()
    {
        isHanging = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
        animRootTarget = null;
    }

     void AnimLerp()
     {
        if(!animRootTarget) return;
   
        if (Vector3.Distance(this.transform.position,animRootTarget.position) > 0.1f)
        {
            this.transform.rotation = Quaternion.Lerp(transform.rotation, 
                                                 animRootTarget.rotation, 
                                                 Time.deltaTime * lerpSpeed);
            this.transform.position = Vector3.Lerp(transform.position, 
                                              animRootTarget.position, 
                                              Time.deltaTime * lerpSpeed);
         }
         else
         {
            this.transform.position = animRootTarget.position;
            this.transform.rotation = animRootTarget.rotation;
         }
        
    }

    void Start()
    {
    	anim = this.GetComponent<Animator>();
        animRootTarget = null;
    }

    void FixedUpdate()
    {
        AnimLerp();
    }

    void Update() 
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        
        if(!isHanging)
            transform.Rotate(0, rotation, 0);

        if(translation != 0)
        {
        	anim.SetBool("isWalking",true);
            anim.SetFloat("speed",translation * 0.5f);
        }
        else
        {
        	anim.SetBool("isWalking",false);
            anim.SetFloat("speed",0);
        }
        if (Input.GetKeyDown("space"))
        {
            anim.SetTrigger("isJumping");
        }

        if (Input.GetKeyDown("w"))
        {
            if (isHanging)
            {
                anim.SetTrigger("isClimbing");
                animRootTarget = null;
            }
        }
    }
}
