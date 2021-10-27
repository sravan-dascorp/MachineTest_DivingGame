using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsManager : MonoBehaviour , IResetable
{
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
