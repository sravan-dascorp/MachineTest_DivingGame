using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obs2DCollider : MonoBehaviour , IResetable
{
    [SerializeField]private GameObject mesh1;
    [SerializeField]private GameObject mesh2;

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();
    // Start is called before the first frame update
    void Start()
    {
        positions.Add(mesh1.transform.position);
        positions.Add(mesh2.transform.position);

        rotations.Add(mesh1.transform.rotation);
        rotations.Add(mesh2.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3 newVelocty = new Vector3(0f, 0f, 30f);

            mesh1.GetComponent<Rigidbody>().velocity = newVelocty;
            newVelocty.z = -newVelocty.z;
            mesh2.GetComponent<Rigidbody>().velocity = newVelocty;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "KnifeCollider")
        {
            KnifeCollider knifeScript = collision.GetComponent<KnifeCollider>();
            knifeScript.PlayerMovement.cutObstacle();

            PlayerAudioManager audioPlayer = knifeScript.PlayerMovement.GetComponent<PlayerAudioManager>();
            audioPlayer.PlayClip(audioPlayer.clips.CutObstacle, 0.7f);

            GetComponent<BoxCollider2D>().enabled = false;

            addBodies();
        }
        else if (collision.name == "HandleCollider")
        {
            collision.GetComponent<HandleCollider>().PlayerMovement.jumpFlip(false);
        }
    }

    private void addBodies()
    {
        Rigidbody body1 = mesh1.AddComponent<Rigidbody>();
        Rigidbody body2 = mesh2.AddComponent<Rigidbody>();
        body1.mass = 10f;
        body2.mass = 10f;
        int randmForce = Random.Range(3, 8);
        Vector3 newVelocty = new Vector3(0f, 0f, randmForce);

        body1.velocity = newVelocty;
        newVelocty.z = -newVelocty.z;
        body2.velocity = newVelocty;
    }

    public void ResetBehavior()
    {
        if(mesh1.GetComponent<Rigidbody>() != null)
        {
            Destroy(mesh1.GetComponent<Rigidbody>());
            Destroy(mesh2.GetComponent<Rigidbody>());
        }

        GetComponent<BoxCollider2D>().enabled = true;

        mesh1.transform.SetPositionAndRotation(positions[0], rotations[0]);
        mesh2.transform.SetPositionAndRotation(positions[1], rotations[1]);
    }
}
