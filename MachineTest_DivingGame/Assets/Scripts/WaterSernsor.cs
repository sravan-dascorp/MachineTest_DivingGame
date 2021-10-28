using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DivingGame.Controller;


public class WaterSernsor : MonoBehaviour
{
   
   
    bool hitWater;
    float waterBedPosition;
    float playerUnderWaterPos;
     private Player _playerScript;
    private RaycastHit _hit;
     private GameObject _waterSurface;
    private GameObject _player;
    bool reachDiveEnd = false;

    [SerializeField] Text diveIndicator;


    float currentYPositon;

    void Awake()
    {
       
        _waterSurface = GameObject.FindGameObjectWithTag("Water Volume");
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<Player>();   
    }
    private void Start() {
         diveIndicator.gameObject.SetActive(false);
    }


    void Update()
    {
        bool isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit, 5f);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.red);


        if (isHit && _hit.collider.gameObject == _waterSurface && !_playerScript.levelfailed)
        {
            _playerScript.levelWon = true;
            
        }

        if(hitWater)
        {
             diveIndicator.gameObject.SetActive(true);
            float updatedYPositon = transform.position.y;
            if(updatedYPositon <= currentYPositon)
            {
                currentYPositon = updatedYPositon;
            }
            else if(updatedYPositon > currentYPositon)
            {
               
                reachDiveEnd=true;
                 print(reachDiveEnd);
            }
            diveIndicator.text = "Underwater traveled Distance : "+  calculateDiveDistance().ToString();
        }
        if(reachDiveEnd) {DivePoint(calculateDiveDistance()); reachDiveEnd=false; hitWater=false;} 

        
    
    }

public void  setHitPosition(float hitpositionY,bool _isHit)
{
    hitWater = _isHit;
    waterBedPosition = hitpositionY;
    currentYPositon = waterBedPosition;
}
    
    float calculateDiveDistance()
    {
        return Mathf.Abs(Mathf.Abs(currentYPositon)- Mathf.Abs(waterBedPosition));
    }

    void DivePoint(float diveDistance)
    {
            if(diveDistance >50) {ScoreManager.instance.AddBonusPoint(100); return;}
             if(diveDistance >40) {ScoreManager.instance.AddBonusPoint(80); return;}
              if(diveDistance >30) {ScoreManager.instance.AddBonusPoint(60); return;}
               if(diveDistance >20) {ScoreManager.instance.AddBonusPoint(40); return;}
                if(diveDistance >10) {ScoreManager.instance.AddBonusPoint(20); return;}
    }


}