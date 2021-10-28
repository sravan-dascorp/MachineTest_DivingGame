using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum PlayerState
{
    idle,falling,Spinning,underwater
}
namespace DivingGame.Controller{
public class Player : MonoBehaviour
{
    [SerializeField] bool useGravity=false;
    [SerializeField] bool useDrag;
    [SerializeField] float dragWhenSpin=1f;
    [SerializeField] float dragNormalState;
     public float force;
    public bool levelWon = false;
    public bool levelfailed = false;
    public Text text;

    public bool isSpinning{get =>  _animator.GetBool("isSpin");}
    

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
    PlayerAudioManager audioplayer;

    bool hitWaterUpdated = false;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _waterSurface = GameObject.FindGameObjectWithTag("Water Volume");
        capCollider =  this.GetComponent<CapsuleCollider>();
        colliderHeight= capCollider.height;
        audioplayer = GetComponent<PlayerAudioManager>();
        

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


        if(isSpinning){transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;  } else transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green; //audioplayer.PlayClip(audioplayer.clips.Flip, 0.7f);

    }




//using drag instead of custom gravity 
 void FixedUpdate() {
            if(_animator.GetBool("isSpin")) _rb.drag=1 ; else  _rb.drag=0;
        }
   
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle") && !isSpinning){
            other.isTrigger = false;
            levelfailed = true;
                text.text = "Smashed ";
                _animator.SetBool("isFall", true);}


          if(other.CompareTag("Obstacle") && isSpinning)audioplayer.PlayClip(audioplayer.clips.BlowObstacle, 0.7f);     
        if (other.gameObject == _waterSurface)
        {
         if(!hitWaterUpdated){   GetComponent<WaterSernsor>().setHitPosition(transform.position.y,true); hitWaterUpdated=true; }
            Camera.main.GetComponent<CameraFollow>().smoothSpeed = 0;
            _isJumping = false;

            if(!levelWon)
            {
                levelfailed = true;
                text.text = "Try Again..";
                _animator.SetBool("isFall", true);
            }

            if(levelWon && !_spinPlayed)
            {
                ScoreManager.instance.AddPoint(5);
                _animator.SetBool("isVictory", true);
                text.text = "Success!";
               _rb.constraints =  RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX ; 
            }

            if(levelWon && _spinPlayed)
            {
                ScoreManager.instance.AddBonusPoint(10);
                _animator.SetBool("isVictory", true);
                text.text = "Spin Point!";
                _rb.constraints =  RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX ; 
            }
        }
       
        
    }

    private void OnCollisionEnter(Collision other) {
          if(other.gameObject !=_waterSurface && other.gameObject !=_startBoard )
          {
       
            print(other.gameObject.name);
            levelfailed = true;
               // text.text = "Try Again..";
                _animator.SetBool("isFall", true);
          }
        
        // if(other.gameObject ==_startBoard && _animator.GetBool("isJumping"))
        // {
        //      _animator.SetBool("isJumping", false); 
        //      _animator.SetBool("isReady", false);
        //       CapsuleCollider collider =  this.GetComponent<CapsuleCollider>();
        //     collider.height = *2; 
             
            
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
}
