using System.Collections.Generic;
using UnityEngine;

public class BlobFactory : MonoBehaviour
{
    [SerializeField] private Blob blobPrefab;
    [SerializeField] private Transform blobParent;
    [SerializeField] private List<Blob> blobList = new List<Blob>();
    [SerializeField] private Transform border;






    public Blob Create(int level/*,Vector3 position = default*/)
    {
        Vector3 position = RandomCoordinate();
        Blob blob = Instantiate(blobPrefab, position, Quaternion.identity, blobParent);
        blob.SetLevel(level);
        blobList.Add(blob);
        return blob;
    }

    public void BlobListClear(Blob blob)
    {
        blobList.Remove(blob);
    }

    public Vector3 RandomCoordinate()
    {
        float x, y, z;
        x = Random.Range(border.GetChild(1).transform.position.x, border.GetChild(0).transform.position.x);
        z = Random.Range(border.GetChild(3).transform.position.z, border.GetChild(0).transform.position.z);
        y = 0f;
        Vector3 blobposition = new Vector3(x, y, z);
        Debug.Log("blob position"+blobposition);
        return blobposition;
    }
    
    
}
