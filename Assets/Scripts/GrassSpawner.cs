using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrassSpawner : MonoBehaviour
{
    [SerializeField] private GameObject grass;
    [SerializeField] private float yOffset = 0.05f;
    [SerializeField] private float radius = 10f;

    [SerializeField] private float maxSpawnTime = 2f;
    private float currTime;

    private void Update()
    {
        currTime -= Time.deltaTime;
        if(currTime <= 0)
        {
            RandomNavmeshLocation(radius);
            currTime = maxSpawnTime;
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        randomDirection.y -= yOffset;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        var obj = Instantiate(grass, finalPosition, Quaternion.identity);

        return finalPosition;
    }
}
