using System.Collections.Generic;
using UnityEngine;

public class PoolingTest : MonoBehaviour
{
    public  List<GameObject> usingProjectiles = new List<GameObject>();
    public  List<GameObject> unusingProjectiles = new List<GameObject>();
    public List<GameObject> projectilePrefabs = new List<GameObject>();

    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            if (unusingProjectiles.Count > 0)
            {
                var projectile = unusingProjectiles[0];
                projectile.SetActive(true);

                usingProjectiles.Add(projectile);
                unusingProjectiles.Remove(projectile);
            }
            else
            {
                var projectile = Instantiate(projectilePrefabs[0]);
                usingProjectiles.Add(projectile);
            }
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        unusingProjectiles.Add(projectile);
        usingProjectiles.Remove(projectile);
    }
}
