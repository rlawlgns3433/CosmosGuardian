using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureTest : MonoBehaviour
{
    public List<GameObject> rt = new List<GameObject>();
    int count = 0;
    private void Update()
    {
        if(Time.frameCount % 3 == 0)
        {
            foreach (GameObject go in rt)
            {
                if (go.activeInHierarchy)
                {
                    ++count;
                }
            }
            Debug.Log(count);
            count = 0;
        }
    }
}