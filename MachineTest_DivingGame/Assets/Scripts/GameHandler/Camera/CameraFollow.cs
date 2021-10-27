using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSidePerspective : MonoBehaviour
{
    private Transform knife;
    [SerializeField] private float offSetX;
    [SerializeField] private float offSetY;

    [SerializeField] private float smoothness;
    // Start is called before the first frame update
    void Start()
    {
        knife = GameObject.FindGameObjectWithTag("Player").transform;   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = knife.position.x + offSetX;
        newPosition.y = knife.position.y + offSetY;

        transform.position = Vector3.MoveTowards(transform.position, newPosition, smoothness);
    }
}
