using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSplashSpawner : MonoBehaviour
{
    public GameObject lavaSplash;

    private void Start()
    {
        StartCoroutine(SpawnLavaSplash());
    }

    IEnumerator SpawnLavaSplash()
    {
        GameObject newLavaSplash = GameObject.Instantiate(lavaSplash);
        newLavaSplash.transform.position = new Vector2(Random.Range(-4f, 4f), -2);
        yield return new WaitForSeconds(4f);
        StartCoroutine(SpawnLavaSplash());
    }
}
