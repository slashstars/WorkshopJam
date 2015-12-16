using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public string inputSet = "P1";
    public float movementSpeed = 2;
    public float jumpForce = 1;
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

        if (!IsGrounded() || !IsMoving())
        {
            anim.SetBool("run", false);
        }
    }

    private void Move(float axis)
    {
        var direction = Mathf.Sign(axis);
        body.AddForce(direction * transform.right * movementSpeed);
        anim.SetBool("run", true);
        Flip(direction);
    }

    private void Flip(float direction)
    {
        if (direction != transform.localScale.x)
        {
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
        }
    }

    private bool IsGrounded()
    {
        return Mathf.Abs(body.velocity.y) < 0.1f;
    }

    private bool IsMoving()
    {
        return Mathf.Abs(body.velocity.x) > 0.1f;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            anim.SetTrigger("jump");
            body.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Fire()
    {
        anim.SetTrigger("melee");
    }

    private void AltFire()
    {
        anim.SetTrigger("fire");
    }
}
