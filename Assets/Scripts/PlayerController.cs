using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Transform bulletPrefab;
    public string inputSet = "P1";
    public float movementSpeed = 2;
    public float jumpForce = 1;
    public float meleeForce = 5;
    public float fireForce = 5;
    public float cooldown = 0;
    private string horizontalAxisName = "Horizontal_";
    private string fireName = "Fire_";
    private string altFireName = "AltFire_";
    private string jumpName = "Jump_";
    private Rigidbody2D body;
    private Animator anim;
    private bool dead = false;
    private float currentCooldownValue;
    //private readonly Vector3 bulletOffset = new Vector3(0.137f, 0.011f, 0);
    private readonly Vector3 bulletOffset = new Vector3(0.1f, 0.011f, 0);

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

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
                Melee();

            if (Input.GetButtonDown(altFireName) && currentCooldownValue <= 0)
                Fire();

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

    private void Melee()
    {
        anim.SetTrigger("melee");
        ResetCooldown();
    }

    private void Fire()
    {
        anim.SetTrigger("fire");

        var position = new Vector3(transform.position.x + transform.localScale.x * bulletOffset.x,
            transform.position.y - bulletOffset.y, 0);
        Transform bullet = (Transform)Instantiate(bulletPrefab, position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * 4, 0, 0), ForceMode2D.Impulse);

        ResetCooldown();
    }

    private void ResetCooldown()
    {
        currentCooldownValue = cooldown;
    }


    public void GetHit(float hitDirection, float pushForce)
    {
        body.AddForce(hitDirection * transform.right * pushForce, ForceMode2D.Impulse);
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
        var other = col.gameObject;
        if (other.tag == Tags.Fire)
        {
            Die();
        }

        if (other.tag == Tags.Sword)
        {
            var hitPlayer = other.transform.parent.gameObject;

            if (gameObject != hitPlayer)
            {
                var hitDirection = Mathf.Sign(transform.position.x - other.transform.position.x);
                GetHit(hitDirection, meleeForce);
            }                
        }

        if (other.tag == Tags.Bullet)
        {
            var hitDirection = Mathf.Sign(other.gameObject.GetComponent<Rigidbody2D>().velocity.x);
            GetHit(hitDirection, fireForce);
        }
    }

    public bool IsDead()
    {
        return dead;
    }
}
