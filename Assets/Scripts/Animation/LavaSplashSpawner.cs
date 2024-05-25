using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSplashSpawner : MonoBehaviour
{
    public GameObject lavaSplash;
    public float interval = 4f;
    public float ypos = -3f;

    private void Start()
    {
        StartCoroutine(SpawnLavaSplash());
    }

    IEnumerator SpawnLavaSplash()
    {
        GameObject newLavaSplash = Instantiate(lavaSplash);
        newLavaSplash.transform.position = new Vector2(Random.Range(-4f, 4f), ypos);
        yield return new WaitForSeconds(interval);
        StartCoroutine(SpawnLavaSplash());
    }
}
