using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] private BlobFactory blobFactory;
   [SerializeField] private float timer = 1f;

   private void Update()
   {
      // if (Input.GetKeyDown(KeyCode.Alpha0))
      //    blobFactory.Create(1);

      timer -= Time.deltaTime;
      if (timer <= 0f)
      {
         blobFactory.Create(1);
         timer = 5f;
      }
   }
}
