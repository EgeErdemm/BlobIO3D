using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Player : MonoBehaviour
{
    private static Player _Instance;
    public static Player Instance => _Instance;


    [SerializeField] private float speed = 3f;
    //Scale Part
    [SerializeField] private Transform _pivot;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private int level = 1;
    [SerializeField] private float StartRadius = 0.15f;
    private const float RadiusPerLevel = 0.01f;
    //
    [SerializeField] private BlobFactory blobFactory;

    public int Level => level;
    private Tween FovTween;
    private float initialFov =20f;

    private GameManager _GameManager;


    #region LifeCycle

    private void Awake()
    {
        _GameManager = GameManager.Instance;
        _Instance = this;
        SetLevel(level);
    }

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
        //speed = _GameManager.SetSpeed(level);
    }

    private void IncreaseLevel(int level)
    {
        SetLevel(this.level +level);
        //Camera.main.fieldOfView += level;
        initialFov += level;
        float duration = 1f;
        FovTween?.Kill();
        FovTween = Camera.main.DOFieldOfView(initialFov, duration);
    }
    
    private void ColletBlob(Blob blob)
    {
        IncreaseLevel(blob.Level);
        //Destroy(blob.gameObject);
        //blobFactory.BlobListClear(blob);
        //yenilen yem farklı konumda spawnlansın
        blob.transform.position = blobFactory.RandomCoordinate();
    }
    
}