using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rigidbodyObject;
    private Collider2D colliderObject;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private static int score;
    private static int health;
    public int numberOfLives;

    public Image[] lives;

    public Sprite liveFull;
    public Sprite liveEmpty;

    SpriteRenderer spriteRenderer;
    private Material blinkMaterial;
    private Material defaultMaterial;

    private UnityEngine.Object destroyEffects;

    public Text scoreText;

    private int extraJumps;
    public int extraJumpsValue;

    public float spawnX, spawnY;

    private void Start()
    {
        extraJumps = extraJumpsValue;
        spawnX = transform.position.x;
        spawnY = transform.position.y;
        rigidbodyObject = GetComponent<Rigidbody2D>();
        colliderObject = GetComponent<Collider2D>();

        if (SceneManager.GetActiveScene().name == "scene1")
        {
            health = numberOfLives;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        blinkMaterial = Resources.Load("Blink", typeof(Material)) as Material;
        defaultMaterial = spriteRenderer.material;

        destroyEffects = Resources.Load("DestroyEffects");

        FindObjectOfType<StoryElement>().TriggerDialogue();

        scoreText.text = "SCORE: " + score;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.IsTouchingLayers(colliderObject, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rigidbodyObject.velocity = new Vector2(moveInput * speed, rigidbodyObject.velocity.y);

        if ((facingRight == false && moveInput > 0) || (facingRight == true && moveInput < 0))
        {
            Flip();
        }
    }

    private void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && extraJumps > 0)
        {
            Invoke("Jump", .1f);
            extraJumps--;
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && extraJumps == 0 && isGrounded == true)
        {
            Invoke("Jump", .1f);
        }

        for (int i = 0; i < lives.Length; i++)
        {
            if (i < health)
            {
                lives[i].sprite = liveFull;
            }
            else
            {
                lives[i].sprite = liveEmpty;
            }

            if (i < numberOfLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }

        scoreText.text = "SCORE: " + score;
    }

    private void Jump()
    {
        FindObjectOfType<PlayerSoundManager>().JumpSound();
        rigidbodyObject.velocity = Vector2.up * jumpForce;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void Hurt()
    {
        spriteRenderer.material = blinkMaterial;
        health--;
        Invoke("ResetMaterial", .05f);
        if (health <= 0)
        {
            GameObject destroyEffectsRef = (GameObject)Instantiate(destroyEffects);
            destroyEffectsRef.transform.position = transform.position;
            Invoke("Death", .05f);
        }
    }
    
    private void ResetMaterial()
    {
        spriteRenderer.material = defaultMaterial;
    }

    private void Death()
    {
        SceneManager.LoadScene("Game Over");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("movingBlock") || collision.gameObject.name.Equals("circleBlock"))
        {
            this.transform.parent = collision.transform;
        }

        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy != null)
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                if (point.normal.y >= 0.6f)
                {
                    enemy.EnemyHurt();
                }
                else
                {
                    rigidbodyObject.velocity = Vector2.up * 10;
                    Hurt();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("movingBlock") || collision.gameObject.name.Equals("circleBlock"))
        {
            this.transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "fish")
        {
            FindObjectOfType<PlayerSoundManager>().ActionSound();
            Destroy(collision.gameObject);
            score++;
        }
        if (collision.gameObject.name == "food")
        {
            FindObjectOfType<PlayerSoundManager>().ActionSound();
            Destroy(collision.gameObject);
            score += 10;
            health++;
        }
        if (collision.gameObject.name == "fishPoisoned")
        {
            FindObjectOfType<PlayerSoundManager>().IncorrectActionSound();
            transform.position = new Vector3(spawnX, spawnY, transform.position.z);
            Destroy(collision.gameObject);
            if (score > 0)
            {
                score--;
            }
        }

        if (collision.gameObject.name == "dieCollider")
        {
            Hurt();
            transform.position = new Vector3(spawnX, spawnY, transform.position.z);
        }

        if (collision.gameObject.name == "endLevel1")
        {
            SceneManager.LoadScene("scene2");
        }
        if (collision.gameObject.name == "endLevel2")
        {
            SceneManager.LoadScene("scene3");
        }
        if (collision.gameObject.name == "endLevel3")
        {
            SceneManager.LoadScene("Win Screen");
        }
    }
}
