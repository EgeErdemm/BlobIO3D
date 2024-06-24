using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager Instance => _Instance;
   [SerializeField] private BlobFactory blobFactory;
   [SerializeField] private int StartingCountOfBlob = 500;
    [SerializeField] private int StartingCountOfEnemy= 10;
    [SerializeField] private int StartingCountOfPoisoned = 30;
    [SerializeField] private float CheckRadius = 5f;
    [SerializeField] private float speedDecreaser = 0.01f;

    [SerializeField] private GameOverScreen _gameOverScreen;

    public float checkRadius => CheckRadius;

    [SerializeField] private Transform border;
    private float maxX, maxZ, minX, minZ;

    //For gameOver
    private bool GameFinished = false;
    public bool isGameFinished => GameFinished;

    private void Start()
    {
  
    }

    private void Awake()
    {
        _Instance = this;
        for(int i=0; i<StartingCountOfBlob; i++)
        {
            Blob blob = blobFactory.Create(1);
            SetRandomMaterialOfBlob(blob);
        }

        for (int j = 0; j < StartingCountOfEnemy; j++)
        {
            Enemy enemy = blobFactory.CreateEnemy(2);
            SetRandomMaterial(enemy);
        }
        for (int k = 0; k < StartingCountOfPoisoned; k++)
        {
            Poisoned poisoned = blobFactory.CreatePoisoned();
        }


        maxX = border.GetChild(0).transform.position.x; //25
        maxZ = border.GetChild(0).transform.position.z; //25
        minX = -border.GetChild(0).transform.position.x; //-25
        minZ = -border.GetChild(0).transform.position.z; //-25


    }

    public bool IsBigger(float player, float enemy)
    {
        if (player > enemy)
        {
            bool IsBig = true;
            return IsBig;
        }
        else
        {
            bool IsBig = false;
            return IsBig;
        }


    }

    public float SetSpeed(float speed /* level de gelmeli */)
    {
        speed -= speedDecreaser /* *level */;
        return speed;
    }


    public void CheckWordLimit(Transform player)
    {
        Vector3 position = player.position;

        bool limitValueNullCheck = border?.GetChild(0)?.transform != null;

        if (limitValueNullCheck)
        {
            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.z = Mathf.Clamp(position.z, minZ, maxZ);
        }

        player.position = position;
    }

    public void SetGameFinished(bool value)
    {
        GameFinished = value;
        if (GameFinished)
        {
            _gameOverScreen.GameOver();
        }
    }

    // farklı renklerde enemy yap

    public Renderer SetRandomMaterial(Enemy enemy)
    {
        var _renderer = enemy.GetComponentInChildren<Renderer>();
        Color randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        _renderer.material.SetColor("_BaseColor", randomColor);
        Debug.Log("set color");
        return _renderer;
    }

    public Renderer SetRandomMaterialOfBlob(Blob blob)
    {
        var _renderer = blob.GetComponentInChildren<Renderer>();
        Color randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        _renderer.material.SetColor("_BaseColor", randomColor);
        Debug.Log("set color");
        return _renderer;
    }

}
