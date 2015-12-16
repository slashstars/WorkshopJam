using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public string inputSet = "P1";
    public float movementSpeed = 2;
    private string horizontalAxisName = "Horizontal_";
    private string fireName = "Fire_";
    private string altFireName = "AltFire_";
    private string jumpName = "Jump_";
    private Rigidbody2D body;
    private Animator anim;
    private float facingDirection;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        facingDirection = Mathf.Sign(transform.localScale.x);

        horizontalAxisName += inputSet;
        fireName += inputSet;
        altFireName += inputSet;
        jumpName += inputSet;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis(horizontalAxisName);
        if (Mathf.Abs(h) > 0.2)
            Move(h);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(fireName))
            Fire();

        if (Input.GetButtonDown(altFireName))
            AltFire();

        if (Input.GetButtonDown(jumpName))
            Jump();
    }

    private void Move(float axis)
    {
        var direction = Mathf.Sign(axis);
        body.AddForce(direction * transform.right * movementSpeed);
        anim.SetBool("run", true);

        if (direction != facingDirection)
        {
            facingDirection = -1 * facingDirection;
            transform.localScale = new Vector2(facingDirection * transform.localScale.x, transform.localScale.y);
        }
    }

    private void Jump()
    {
        anim.SetTrigger("jump");
    }

    private void Fire()
    {
        anim.SetTrigger("hit");
    }

    private void AltFire()
    {
        anim.SetTrigger("dead");
    }
}
