using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DivingGame.Controller;
namespace DivingGame.Obstacles{
public class Obs3DCollider : MonoBehaviour 
{
    [SerializeField]private GameObject mesh1;
    [SerializeField]private GameObject mesh2;

    public bool changeColorOnHit=false;
    public int hitPoint=10;

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();

     PlayerAudioManager audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        SetPositions();
       // audioPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudioManager> () ;

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            Debug.Log("yes it hit it");
        
          
           // audioPlayer.PlayClip(audioPlayer.clips.CutObstacle, 0.7f);

           

           if(other.GetComponent<Player>().isSpinning) {GetComponent<BoxCollider>().enabled = false; SplitStuff(); ScoreManager.instance.AddPoint(hitPoint) ;  } else
           {
               GetComponent<BoxCollider>().isTrigger=false;
           }
        }
       
    }

    

    public  void SplitStuff()
    {
       
        Rigidbody body1 = mesh1.AddComponent<Rigidbody>();
        Rigidbody body2 = mesh2.AddComponent<Rigidbody>();
        body1.angularDrag=0;
        body1.drag=0;
        body2.angularDrag=0;
        body2.drag=0;
        body1.mass = 10f;
        body2.mass = 10f;
        int randmForce = Random.Range(30, 30);
        Vector3 newVelocty = new Vector3(-randmForce, 0f, 0);

        body1.velocity = newVelocty;
        newVelocty.x = -newVelocty.x;
        body2.velocity = newVelocty;
      if(changeColorOnHit) { mesh1.GetComponent<Renderer>().material.color = Color.red;
         mesh2.GetComponent<Renderer>().material.color = Color.red;}
         StartCoroutine(mesh1.GetComponent<TriangleExplosion>().SplitMesh(true));
        StartCoroutine(mesh2.GetComponent<TriangleExplosion>().SplitMesh(true));
    }

    // public void ResetBehavior()
    // {
    //     if(mesh1.GetComponent<Rigidbody>() != null)
    //     {
    //         Destroy(mesh1.GetComponent<Rigidbody>());
    //         Destroy(mesh2.GetComponent<Rigidbody>());
    //     }

    //     GetComponent<BoxCollider2D>().enabled = true;

    //     mesh1.transform.SetPositionAndRotation(positions[0], rotations[0]);
    //     mesh2.transform.SetPositionAndRotation(positions[1], rotations[1]);
    // }


    void SetPositions()
    {
        positions.Add(mesh1.transform.position);
        positions.Add(mesh2.transform.position);

        rotations.Add(mesh1.transform.rotation);
        rotations.Add(mesh2.transform.rotation);
    }
}
}
