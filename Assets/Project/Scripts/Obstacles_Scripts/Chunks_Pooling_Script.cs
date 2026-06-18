using System.Collections.Generic;
using UnityEngine;

public class Chunks_Pooling_Script : MonoBehaviour
{
    public GameObject[] chunks_Prefabs;
    public List<InfoChunk> chunks_List = new List<InfoChunk>();

    public void SpawnNewChunk()
    {
        int random = Random.Range(0, chunks_Prefabs.Length);
        Vector3 p = chunks_List[chunks_List.Count - 1].transform_Final.position;
        GameObject gameObject = Instantiate(chunks_Prefabs[random], p, Quaternion.identity);
        chunks_List.Add(gameObject.GetComponent<InfoChunk>());
        GameObject temp = chunks_List[0].gameObject;
        chunks_List.RemoveAt(0);
        Destroy(temp);
    }
}
