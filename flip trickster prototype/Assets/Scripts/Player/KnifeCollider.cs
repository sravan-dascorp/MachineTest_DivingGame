using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCollider : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    private BoxCollider2D boxCollider;

    private LayerMask normalLayer;
    private LayerMask noCollisionLayer;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && !boxCollider.isTrigger)
        {
            PlayerMovement.KnifeTouchingGround(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            PlayerMovement.KnifeTouchingGround(false);
        }
    }

    public void ignoreCollions(bool x)
    {
        if (x)
        {
            gameObject.layer = 6;
        }
        else
        {
            gameObject.layer = 0;
        }
    }

}
