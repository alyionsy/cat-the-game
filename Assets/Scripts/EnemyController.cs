using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rb;

    SpriteRenderer spriteRenderer;
    private Material blinkMaterial;
    private Material defaultMaterial;

    private UnityEngine.Object destroyEffects;

    private bool facingRight = true;

    [SerializeField]
    public Transform[] points = new Transform[2];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        blinkMaterial = Resources.Load("Blink", typeof(Material)) as Material;
        defaultMaterial = spriteRenderer.material;

        destroyEffects = Resources.Load("DestroyEffects");
    }

    void Update()
    {

        if (transform.position.x < points[0].position.x)
        {
            Flip();
        }
        else if (transform.position.x > points[1].position.x)
        {
            Flip();
        }

        if (facingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void EnemyHurt()
    {
        spriteRenderer.material = blinkMaterial;
        Invoke("DestroyEnemy", .1f);
    }

    private void DestroyEnemy()
    {
        GameObject destroyEffectsRef = (GameObject) Instantiate(destroyEffects);
        destroyEffectsRef.transform.position = transform.position;
        Destroy(this.gameObject);
    }

}
