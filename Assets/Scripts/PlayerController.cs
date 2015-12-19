using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public string inputSet = "P1";
    public float movementSpeed = 2;
    public float jumpForce = 1;
    public float meleeForce = 1;
    public float cooldown = 2;
    private string horizontalAxisName = "Horizontal_";
    private string fireName = "Fire_";
    private string altFireName = "AltFire_";
    private string jumpName = "Jump_";
    private Rigidbody2D body;
    private Animator anim;
    private float facingDirection;
    private bool dead = false;
    private float currentCooldownValue;

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

        currentCooldownValue = 0;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis(horizontalAxisName);
        if (Mathf.Abs(h) > 0.2 && !dead)
            Move(h);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {

            if (Input.GetButtonDown(fireName) && currentCooldownValue <= 0)
                Fire();

            if (Input.GetButtonDown(altFireName) && currentCooldownValue <= 0)
                AltFire();

            if (Input.GetButtonDown(jumpName))
                Jump();

            if (!IsGrounded() || !IsMoving())
            {
                anim.SetBool("run", false);
            }
        }

        if (currentCooldownValue > 0)
            currentCooldownValue -= Time.deltaTime;
    }

    private void Move(float axis)
    {
        var direction = Mathf.Sign(axis);
        body.AddForce(direction * transform.right * movementSpeed);
        anim.SetBool("run", true);
        Flip(direction);
    }

    public void Flip(float direction)
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
        ResetCooldown();
    }

    private void AltFire()
    {
        anim.SetTrigger("fire");
    }

    private void ResetCooldown()
    {
        currentCooldownValue = cooldown;
    }


    public void GetHit(GameObject otherPlayer)
    {
        var hitDirection = Mathf.Sign(transform.position.x - otherPlayer.transform.position.x);
        body.AddForce(hitDirection * transform.right * meleeForce, ForceMode2D.Impulse);
        anim.SetTrigger("hit");

        Flip(hitDirection);
    }

    public void Die()
    {
        dead = true;
        anim.SetTrigger("dead");
    }

    public void Revive()
    {
        dead = false;
        anim.SetTrigger("revive");
    }

    public void Disarm()
    {
        //Disarm;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == Tags.Fire)
        {
            Die();
        }

        if (col.gameObject.tag == Tags.Sword)
        {
            var hitPlayer = col.gameObject.transform.parent.gameObject;
            if (gameObject != hitPlayer)
                GetHit(hitPlayer);
        }
    }

    public bool IsDead()
    {
        return dead;
    }
}
