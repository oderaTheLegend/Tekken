using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] Vector3[] spawnPos;
    [SerializeField] float spawnTime = 0.3f;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            int index = Random.Range(0, spawnPos.Length);

            Instantiate(prefab, spawnPos[index], Quaternion.identity);
            timer = 0;
        }
    }
}
