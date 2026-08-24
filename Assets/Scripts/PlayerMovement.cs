using System.Collections;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float jumpIndex;
    public Rigidbody2D rb;
    public float hor;

    public int horLookDir;
    public float dashForce;
    public int dashDuration;
    public bool isDashing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpIndex = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Dash();
        }

        if (isDashing)
            return;

        if (Input.GetKeyDown(KeyCode.J) && jumpIndex > 0)
        {
            jumpIndex--;
            Jump();
        }

        hor = Input.GetAxis("Horizontal");
        transform.Translate(new Vector2(hor * speed * Time.deltaTime, 0));

        switch (hor)
        {
            case (> 0):
                horLookDir = 1;
                break;

            case (< 0):
                horLookDir = -1;
                break;
        }

    }

    private async void Jump()
    {
        if (isDashing) return;

        rb.linearVelocityY =  1.5f * jumpForce;
    }

    private async void Dash()
    {
        isDashing = true;
        var originalGravity = rb.gravityScale;

        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        rb.linearVelocityX = horLookDir * dashForce;

        await Task.Delay(dashDuration);

        rb.gravityScale = originalGravity;
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            rb.linearVelocityX = 0;
            jumpIndex = 2;
        }
    }
}
