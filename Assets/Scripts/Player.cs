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
    private Transform _transform;
    //
    //[SerializeField] private BlobFactory _blobFactory;

    public int Level => level;
    private Tween FovTween;
    private float initialFov =20f;
    private float currentFov = 0;
    private float maxFov = 50f;

    private GameManager _GameManager;
    private BlobFactory _blobFactory;

    #region LifeCycle

    private void Awake()
    {

        _Instance = this;

    }


    private void Start()
    {
        _transform = transform;
        _blobFactory = BlobFactory.Instance;
        _GameManager = GameManager.Instance;
        SetLevel(level);
    }

    private void Update()
    {
        if (_GameManager == null)
            Debug.Log("gamemanager null");
        if (_blobFactory == null)
            Debug.Log("blob factory null");
        Vector3 mousePosition = Input.mousePosition;
        
        Vector3 normalizedTargetPosition = new Vector3(
            (mousePosition.x / Screen.width) * 2 - 1, 
            transform.position.y,                     
            (mousePosition.y / Screen.height) * 2 - 1  
        );
        //Movement
        Vector3 targetPosition = _transform.position + normalizedTargetPosition;
        _transform.LookAt(targetPosition);
        _transform.position += normalizedTargetPosition * (Time.deltaTime * speed);
        // Scale
        SetScale(level);
        _GameManager?.CheckWordLimit(gameObject.transform);

    }

    private void OnTriggerEnter(Collider other)
    {
       if( other.TryGetComponent(out Blob blob) )
           ColletBlob(blob);


        if (other.TryGetComponent(out Enemy enemy))
        {


            bool thisBig = _GameManager.IsBigger(level, enemy._Level);
            if (thisBig)
            {
                ColletEnemy(enemy);

            }
        }

        if (other.TryGetComponent(out Poisoned poisoned))
        {
            _GameManager.SetGameFinished(true);
            poisoned.transform.position = _blobFactory.RandomCoordinate();
        }

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
        currentFov = initialFov + this.level;
        currentFov = Mathf.Clamp(currentFov, initialFov, maxFov);
        float duration = 1f;
        FovTween?.Kill();
        FovTween = Camera.main.DOFieldOfView(currentFov, duration);
    }
    
    private void ColletBlob(Blob blob)
    {
        IncreaseLevel(blob.Level);
        //Destroy(blob.gameObject);
        //blobFactory.BlobListClear(blob);
        //yenilen yem farklı konumda spawnlansın
        blob.transform.position = _blobFactory.RandomCoordinate();
    }

    private void ColletEnemy(Enemy enemy)
    {
        IncreaseLevel(enemy._Level);
        enemy.transform.position = _blobFactory.RandomCoordinate();
        enemy.SetLevel(2);
    }


}