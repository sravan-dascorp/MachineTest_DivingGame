using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCollider : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            PlayerMovement.jumpFlip(false);
        }
    }
}
