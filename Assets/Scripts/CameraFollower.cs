using System;
using Unity.VisualScripting;
using UnityEngine;


public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform mTarget;
    [SerializeField] private Vector3 mOffset = new Vector3(0, 14, 0);
    [SerializeField] private float mSpeed = 5f;
    private Vector3 m_velocity = Vector3.zero;
    [SerializeField] private MovementType movementType = MovementType.Lerp;



    
    
    
    
    private void Update()
    {
        if(mTarget ==null) return;
        Vector3 targetPos = mTarget.position + mOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, mSpeed * Time.deltaTime);
        
        switch(movementType)
        {
            case MovementType.Lerp:
                transform.position = MoveWithLerp(targetPos);
                break;
            case MovementType.Slerp:
                transform.position = MoveWithSlerp(targetPos);
                break;
            case MovementType.MoveTowards:
                transform.position = MoveWithMoveTowards(targetPos);
                break;
            case MovementType.SmoothDamp:
                transform.position = MoveWithSmoothDamp(targetPos);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }



    }

    
    
    
    
    
    
    
    
    #region MovementType
        
    public enum MovementType
    {
        Lerp,
        Slerp,
        MoveTowards,
        SmoothDamp
    };
    
    private Vector3 MoveWithLerp(Vector3 targetPosition)
    {
        return Vector3.Lerp(transform.position, targetPosition, mSpeed * Time.deltaTime);
    }

    private Vector3 MoveWithSlerp(Vector3 targetPosition)
    {
        return Vector3.Slerp(transform.position, targetPosition, mSpeed * Time.deltaTime);
    }

    private Vector3 MoveWithMoveTowards(Vector3 targetPosition)
    {
        return Vector3.MoveTowards(transform.position, targetPosition, mSpeed * Time.deltaTime);
    }

    private Vector3 MoveWithSmoothDamp(Vector3 targetPosition)
    {
        return Vector3.SmoothDamp(transform.position, targetPosition,ref m_velocity,mSpeed);

    }
    

    #endregion


    
}

