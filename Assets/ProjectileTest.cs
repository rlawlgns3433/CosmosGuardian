using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    public GameObject prefab;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {

            Instantiate(prefab);
        }
    }
}
