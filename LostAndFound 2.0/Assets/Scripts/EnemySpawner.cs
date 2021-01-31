using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject deadEnemy;
    public GameObject[] items;
    public float itemChance;
    public Vector2 location;
    public bool spawnEnemy;
    public List<GameObject> livingEnemies;
    public List<GameObject> deadEnemies;
    public List<GameObject> spawnedItems;
    public bool clean;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnEnemy)
        {
            SpawnEnemy(location);
            spawnEnemy = false;
        }

        if (clean)
        {
            ClearRoom();
            clean = false;
        }
    }

    public void SpawnEnemy(Vector2 loc)
    {
        GameObject enemy = enemies[Random.Range(0, enemies.Length)];
        if (enemy)
        {
            livingEnemies.Add(Instantiate(enemy, loc, Quaternion.identity));
            livingEnemies[livingEnemies.Count - 1].GetComponent<Health>().es = this;
        }
    }

    public void ClearRoom()
    {
        int count = 0;
        while(livingEnemies.Count != 0 && count < 10000)
        {
            GameObject target = livingEnemies[0];
            livingEnemies.RemoveAt(0);
            Destroy(target);
            count++;
        }

        count = 0;
        while (deadEnemies.Count != 0 && count < 10000)
        {
            GameObject target = deadEnemies[0];
            deadEnemies.RemoveAt(0);
            Destroy(target);
            count++;
        }

        count = 0;
        while (spawnedItems.Count != 0 && count < 10000)
        {
            GameObject target = spawnedItems[0];
            spawnedItems.RemoveAt(0);
            Destroy(target);
            count++;
        }
    }

    public void Kill(GameObject enemy)
    {
        Vector3 pos = enemy.transform.position;
        livingEnemies.Remove(enemy);
        deadEnemies.Add(Instantiate(deadEnemy, pos, Quaternion.identity));
        deadEnemies[deadEnemies.Count - 1].transform.localScale = (Vector3.right + Vector3.up) * 3.5f;
        if(Random.Range(1,100) <= itemChance)
        {
            GameObject item = items[Random.Range(0, items.Length)];
            if (item)
            {
                spawnedItems.Add(Instantiate(item, pos, Quaternion.identity));
            }
        }
        Destroy(enemy);

    }
}
