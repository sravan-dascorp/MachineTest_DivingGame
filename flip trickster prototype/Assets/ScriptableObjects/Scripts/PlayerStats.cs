using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] float horizontalSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float fastRotateSpeed;
    [SerializeField] float slowRotateSpeed;

    [SerializeField] private Vector3 desiredRotationInit;
    [SerializeField] private Vector3 desiredRotationFinal;
    [SerializeField] private Vector3 horizontalRotation;

    public float getHorizontalSpeed()
    {
        return horizontalSpeed;
    }
    public float getJumpForce()
    {
        return jumpForce;
    }
    public float getFastRotateSpeed()
    {
        return fastRotateSpeed;
    }

    public float getSlowRotateSpeed()
    {
        return slowRotateSpeed;
    }

    public Vector3 getDesiredRotationInit()
    {
        return desiredRotationInit;
    }

    public Vector3 getDesiredRotationFinal()
    {
        return desiredRotationFinal;
    }

    public Vector3 getHorizontalRotation()
    {
        return horizontalRotation;
    }
}
