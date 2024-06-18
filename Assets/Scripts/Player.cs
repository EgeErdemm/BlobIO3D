using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    //Scale Part
    [SerializeField] private Transform _pivot;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private int level = 1;
    [SerializeField] private float StartRadius = 0.15f;
    private const float RadiusPerLevel = 0.01f;
    //
    [SerializeField] private BlobFactory blobFactory;

    public int Level => level;

    
    #region LifeCycle

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        Vector3 normalizedTargetPosition = new Vector3(
            (mousePosition.x / Screen.width) * 2 - 1, 
            transform.position.y,                     
            (mousePosition.y / Screen.height) * 2 - 1  
        );
        //Movement
        Vector3 targetPosition = transform.position + normalizedTargetPosition;
        transform.LookAt(targetPosition);
        transform.position += normalizedTargetPosition * (Time.deltaTime * speed);
        // Scale
        SetScale(level);
    }

    private void OnTriggerEnter(Collider other)
    {
       if( other.TryGetComponent(out Blob blob) )
           ColletBlob(blob);
    }
    

    #endregion

    public void SetScale(float level)
    {
        float radius = (level - 1) * RadiusPerLevel + StartRadius;
        float scale = radius * 2;
        _pivot.localScale = scale * Vector3.one;
        _sphereCollider.radius = radius;
        Vector3 center = _sphereCollider.center;
        center.y = radius;
        _sphereCollider.center = center;
    }
    
    public void SetLevel(int level)
    {
        this.level = level;
        SetScale(level);
    }

    private void IncreaseLevel(int level)
    {
        SetLevel(this.level +level);
        Camera.main.fieldOfView += level;
    }
    
    private void ColletBlob(Blob blob)
    {
        IncreaseLevel(blob.Level);
        Destroy(blob.gameObject);
        blobFactory.BlobListClear(blob);
    }
    
}