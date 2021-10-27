using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour , IResetable
{
    private Transform player;

    private Vector3 playerPosition;
    private Quaternion playerRotation;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPosition = player.position;
        playerRotation = player.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBehavior()
    {
        foreach (Transform child in transform)
        {
            IResetable reset = child.GetComponent<IResetable>();
            if (reset != null) reset.ResetBehavior();
        }

        player.SetPositionAndRotation(playerPosition, playerRotation);
        player.GetComponent<PlayerMovement>().KnifeTouchingGround(true);
    }
}
