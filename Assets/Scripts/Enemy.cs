using UnityEngine;
using DG.Tweening;
using System;

public class Enemy : MonoBehaviour
{
    private Player _Player;
    private GameManager _GameManager;
    private BlobFactory _blobFactory;

    [SerializeField]private float speed = 3f;
    [SerializeField]private int level = 10;
    public int _Level => level;
    public float _initialSpeed => speed;

    private float durationOfLookAt = 0.3f;
    //[SerializeField] private float durationOfMove = 2f;

    [SerializeField] private float StartRadius = 0.15f;
    [SerializeField] private Transform _pivot;
    [SerializeField] private SphereCollider _sphereCollider;
    private const float RadiusPerLevel = 0.01f;

    Blob closestBlob = null;
    Enemy closestEnemy = null;
    float closestDistance = Mathf.Infinity;
    float closestEnemyDistance = Mathf.Infinity;

    private Transform _transform;
    private Tween _DoLookTween;


    private void Start()
    {
        _transform = transform;
        _Player = Player.Instance;
        _GameManager = GameManager.Instance;
        _blobFactory = BlobFactory.Instance;
        //SetLevel(level);
    }

    private void OnDrawGizmos()
    {
        if (_GameManager == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_transform.position, _GameManager.checkRadius);
    }


    private void Update()
    {
        DoSomethink();
        _GameManager?.CheckWordLimit(gameObject.transform);

    }



    private void DoSomethink()
    {
        bool PlayerInRange = IsPlayerInRange();

        if (PlayerInRange)
        {
            bool MyLevelBigger = IsMyLevelBigger();
            if (MyLevelBigger)
            {
                CatchPlayer();
            }
            else
            {
                Run(_Player.transform);
            }
           
        }
        else
        {
            FindFeed();
        }

    }

    private void FindFeed()
    {
        //Debug.Log("find feed");
        Collider[] colliders = Physics.OverlapSphere(_transform.position, _GameManager.checkRadius);

        foreach(Collider c in colliders)
        {
            // kendi colliderimi atla
            if (c == GetComponent<Collider>())
            {
                continue;
            }

            if (c.TryGetComponent(out Blob blob))
            {
                float distance = Vector3.Distance(_transform.position, blob.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBlob = blob;
                }

            }

            //
            if (c.TryGetComponent(out Enemy enemy))
            {
                float EnemyDistance = Vector3.Distance(_transform.position, enemy.transform.position);
                if (EnemyDistance < closestEnemyDistance && level > enemy._Level)
                {
                    closestEnemyDistance = EnemyDistance;
                    closestEnemy = enemy;
                }
                else if(level < enemy._Level)
                {
                    Run(enemy.transform);
                    return;
                }

            }

        }


        if (closestBlob == null && closestEnemy == null)
        {
            return;
        }
        else if (closestBlob !=null && closestEnemy == null)
        {
            if (_DoLookTween != null)
            {
                _DoLookTween.Kill();
            }

            _DoLookTween = transform.DOLookAt(closestBlob.transform.position, durationOfLookAt);
            Vector3 direction = closestBlob.transform.position - transform.position;
            _transform.position += direction.normalized * Time.deltaTime * speed;
            Debug.Log("go blob");

        }
        else if(closestEnemy != null)
        {
            if (_DoLookTween != null)
            {
                _DoLookTween.Kill();
            }

            _DoLookTween = _transform.DOLookAt(closestEnemy.transform.position, durationOfLookAt);
            Vector3 direction = closestEnemy.transform.position - transform.position;
            _transform.position += direction.normalized * Time.deltaTime * speed;
        }
        //Debug.Log("closestBlob: " + (closestBlob != null ? closestBlob.name : "null"));
        //Debug.Log("closestEnemy: " + (closestEnemy != null ? closestEnemy.name : "null"));

    }

    private void Run(Transform player)
    {

        if (_DoLookTween != null)
        {
            _DoLookTween.Kill();
        }

        Vector3 direction = _transform.position - player.position;
        _DoLookTween = transform.DOLookAt(_transform.position + direction, durationOfLookAt);
        //transform.DOMove(direction, durationOfMove);
        _transform.position += direction.normalized * Time.deltaTime * speed;
        //Debug.Log("Run boy run");
    }

    private void CatchPlayer()
    {


        if(_DoLookTween != null)
        {
            _DoLookTween.Kill();
        }


        Vector3 direction = _Player.transform.position - _transform.position;
        _DoLookTween = _transform.DOLookAt(_Player.transform.position, durationOfLookAt);
        _transform.position += direction.normalized * Time.deltaTime *speed;
        //transform.DOMove(_Player.transform.position, durationOfMove);

        

    }









    private bool IsPlayerInRange()
    {
        return Vector3.Distance(_transform.position, _Player.transform.position)<_GameManager.checkRadius;
    }

    private bool IsMyLevelBigger()
    {

        return _GameManager.IsBigger(level, _Player.Level);

    }






    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Blob blob))
        {
            ColletBlob(blob);
            closestBlob = null;
            closestDistance = Mathf.Infinity;

        }
        if (other.TryGetComponent(out Player player))
        {

            //gameOver
            if (_GameManager.IsBigger(level, _Player.Level))
            {
                _GameManager.SetGameFinished(true);
            }
        }

        if (other.TryGetComponent(out Enemy enemy))
        {

            
           bool thisBig = _GameManager.IsBigger(this._Level, enemy._Level);
            if (thisBig)
            {
                ColletEnemy(enemy);
                closestEnemy = null;
                closestEnemyDistance = Mathf.Infinity;
                //Destroy(enemy);
            }
        }

        if(other.TryGetComponent(out Poisoned poisoned))
        {
            poisoned.transform.position = _blobFactory.RandomCoordinate();
            _transform.position = _blobFactory.RandomCoordinate();
            level = 2;
            SetLevel(level);
        }


    }

    private void ColletEnemy(Enemy enemy)
    {
        IncreaseLevel(enemy.level);
        enemy.transform.position = _blobFactory.RandomCoordinate();
        enemy.level = 2;
        enemy.SetLevel(enemy._Level);
    }

    private void ColletBlob(Blob blob)
    {
        IncreaseLevel(blob.Level);
        blob.transform.position = _blobFactory.RandomCoordinate();
    }
    private void IncreaseLevel(int level)
    {
        SetLevel(this.level + level);
    }
    public void SetLevel(int level)
    {
        this.level = level;
        SetScale(level);
        if(_GameManager != null)
        speed = _GameManager.SetSpeed(speed);
    }

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

}
