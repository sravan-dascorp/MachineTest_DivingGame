using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour
{
    public float force;
    public bool isWin = false;
    public bool isLost = false;
    public Text text;
    

    private Rigidbody _rb;
    private Animator _animator;
    private float _touchStartTime = 0f;
    private GameObject _waterSurface;
    public GameObject _startBoard;
    private Vector3 _currentCamPos;
    private bool _isJumping = false;
    [SerializeField]
    private float _maxForceThreshHold = 500;
    private bool _spinPlayed = false;
    
    [SerializeField] GameObject balancer;  //used to allgn centerofGravity to feet to make player float upright
    CapsuleCollider capCollider; 

    float colliderHeight;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _waterSurface = GameObject.FindGameObjectWithTag("Water Volume");
        capCollider =  this.GetComponent<CapsuleCollider>();
        colliderHeight= capCollider.height;

    }


    void Update()
    {
        //if(!Input.GetMouseButtonDown(0) && _isJumping)  _animator.SetBool("isFall", true);

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            _touchStartTime = Time.time;
            _animator.SetBool("isReady", true);

            if(_isJumping)
            {
                _animator.SetBool("isSpin", true);
                
            }
        }
        

        if (Input.GetMouseButtonUp(0) && !IsPointerOverUIObject())
        {
            float delta = Time.time - _touchStartTime;
            if(delta < 0.3f) delta=0.3f;
            float adjustedForce = force * delta;
            if(adjustedForce > _maxForceThreshHold)
            {
                adjustedForce = _maxForceThreshHold;
            }
            
            Vector3 pos = new Vector3(0, 1, 1);
            _rb.AddForceAtPosition(pos * adjustedForce, Vector3.up * -5);
 

            force = 0;
            _isJumping = true;
            _animator.SetBool("isSpin", false);
            _animator.SetBool("isJumping", true); 
            // CapsuleCollider collider =  this.GetComponent<CapsuleCollider>();
            // collider.height = collider.height/2; 
   
        }
         if(_animator.GetBool("isSpin")) capCollider.height= colliderHeight * 0.5f; else if(!_animator.GetBool("isSpin")&& _spinPlayed  ) capCollider.height =colliderHeight;
        

        
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Spinning") && 
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && 
            !_animator.IsInTransition(0))
            {
                _spinPlayed = true;
            }

        _currentCamPos = Camera.main.transform.position;

    }

 void FixedUpdate() {
            if(_animator.GetBool("isSpin")) _rb.drag=1; else  _rb.drag=0;
        }
   
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _waterSurface)
        {
            Camera.main.GetComponent<CameraFollow>().smoothSpeed = 0;
            _isJumping = false;

            if(!isWin)
            {
                isLost = true;
                text.text = "Try Again..";
                _animator.SetBool("isFall", true);
            }

            if(isWin && !_spinPlayed)
            {
                ScoreManager.instance.AddPoint();
                _animator.SetBool("isVictory", true);
                text.text = "Success!";
               _rb.constraints =  RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX ; 
            }

            if(isWin && _spinPlayed)
            {
                ScoreManager.instance.AddBonusPoint();
                _animator.SetBool("isVictory", true);
                text.text = "Bonus Point!";
                _rb.constraints =  RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX ; 
            }
        }
       
        
    }

    private void OnCollisionEnter(Collision other) {
          if(other.gameObject !=_waterSurface && other.gameObject !=_startBoard && other.transform.tag!= "Obstacle" && other.transform.tag!= "Obstacle");
        {
            isLost = true;
                text.text = "Try Again..";
                _animator.SetBool("isFall", true);
        }
        // if(other.gameObject ==_startBoard && _animator.GetBool("isJumping"))
        // {
        //      _animator.SetBool("isJumping", false); 
        //      _animator.SetBool("isReady", false);
        //       CapsuleCollider collider =  this.GetComponent<CapsuleCollider>();
        //     collider.height = collider.height*2; 
             
            
        // }
    }
    /// <summary>
    /// OnCollisionStay is called once per frame for every collider/rigidbody
    /// that is touching rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
   


    public void FlipPlayer()
    {
        transform.localScale = new Vector3(transform.localScale.x, 
            transform.localScale.y, 
            -1 * transform.localScale.z);
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
