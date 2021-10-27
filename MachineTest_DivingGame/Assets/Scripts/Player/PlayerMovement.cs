using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerAudioManager audioManager;
    [SerializeField] private KnifeCollider knifeColliderScript;

    [SerializeField] PlayerStats stats;

    [SerializeField]private float rotateSpeed;

    private Vector3 targetRotationInit;
    private Vector3 targetRotationFinal;
    private Vector3 horizontalRotation;

    [SerializeField] private bool isManualFlipping;

    [SerializeField]private bool touchingGround;

    private bool isCutting;
    private float cuttingTimer;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        audioManager = GetComponent<PlayerAudioManager>();

        targetRotationInit = stats.getDesiredRotationInit();
        targetRotationFinal = stats.getDesiredRotationFinal();
        horizontalRotation = stats.getHorizontalRotation();

        rotateSpeed = stats.getFastRotateSpeed();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            jumpFlip(true);
        }
    }

    private void FixedUpdate()
    {
        if (!touchingGround)
        {
            if (isCutting)
            {
                if (Vector3.Distance(transform.eulerAngles, horizontalRotation) > 5f)
                {
                    rotateSpeed = stats.getSlowRotateSpeed();
                    transform.Rotate(new Vector3(rotateSpeed, 0, 0));
                }
            }
            else handleRotation();
        }
    }

    public void jumpFlip(bool x)
    {
        isManualFlipping = true;
        rotateSpeed = stats.getFastRotateSpeed();

        if (isCutting)
        {
            isCutting = false;
            cuttingTimer = 0;
        }

        StartCoroutine(disableCollider());
        if (x)
        {
            audioManager.PlayClip(audioManager.clips.Flip, 0.5f);
            body.velocity = new Vector2(stats.getHorizontalSpeed(), stats.getJumpForce());
        }
        else
        {
            audioManager.PlayClip(audioManager.clips.HandleHit, 0.5f);
            body.velocity = new Vector2(-stats.getHorizontalSpeed(), stats.getJumpForce());
        } 
    }

    private void handleRotation()
    {
        if (!isManualFlipping)
        {
            if (Vector3.Distance(transform.eulerAngles, targetRotationInit) < 12f)
            {
                rotateSpeed = stats.getFastRotateSpeed();
            }
            else if (Vector3.Distance(transform.eulerAngles, targetRotationFinal) < 12f)
            {
                rotateSpeed = stats.getSlowRotateSpeed();
            }
            rotateKnife();
        }
        else
        {
            if (Vector3.Distance(transform.eulerAngles, targetRotationInit) < 12f)
            {
                isManualFlipping = false;
            }
            else rotateKnife();
        }
    }

    private void rotateKnife()
    {
        transform.Rotate(new Vector3(rotateSpeed, 0, 0));
    }

    public void KnifeTouchingGround(bool x)
    {
        if (x)
        {
            body.velocity = Vector2.zero;
            body.gravityScale = 0f;
        }
        else
        {
            body.gravityScale = 4f;
        }
        touchingGround = x;
    }

    private IEnumerator disableCollider()
    {
        knifeColliderScript.ignoreCollions(true);
        Vector3 targetRotation = new Vector3(290f, 270f, 180f);
        while(Vector3.Distance(transform.eulerAngles, targetRotation) >= 12f)
        {
            yield return null;
        }
        knifeColliderScript.ignoreCollions(false);
    }


    public void cutObstacle()
    {
        if(body.velocity.y < 0)
        {
            if (!isCutting)
            {
                StartCoroutine(holdRotationCutting());
            }
            else cuttingTimer += 0.1f;
        }
    }
    private IEnumerator holdRotationCutting()
    {
        isCutting = true;
        cuttingTimer = 0.2f;
        body.velocity = new Vector2(body.velocity.x / 3f, body.velocity.y);
        while (cuttingTimer >= 0)
        {
            cuttingTimer -= Time.deltaTime;
            yield return null;
        }
        isCutting = false;
    }

}

