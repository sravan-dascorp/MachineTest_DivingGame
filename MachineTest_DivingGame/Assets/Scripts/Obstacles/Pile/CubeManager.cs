using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour , IResetable
{
    private Obs2DCollider obs2DCollider;
    // Start is called before the first frame update
    void Start()
    {
        obs2DCollider = transform.Find("2DCollider").GetComponent<Obs2DCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBehavior()
    {
        obs2DCollider.ResetBehavior();
    }
}
