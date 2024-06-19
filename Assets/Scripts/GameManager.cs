using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] private BlobFactory blobFactory;
   [SerializeField] private float timer = 1f;
   [SerializeField] private int StartingCountOfBlob = 500;




    private void Awake()
    {
        for(int i=0; i<=StartingCountOfBlob; i++)
        {
            blobFactory.Create(1);
        }
    }



    private void Update()
   {


      //timer -= Time.deltaTime;
      //if (timer <= 0f)
      //{
      //   blobFactory.Create(1);
      //   timer = 5f;
      //}
   }
}
