using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSplashSpawner : MonoBehaviour
{
    public GameObject lavaSplash;
    public float ypos = -3f;

    private void Start()
    {
        StartCoroutine(SpawnLavaSplash());
    }

    IEnumerator SpawnLavaSplash()
    {
        GameObject newLavaSplash = GameObject.Instantiate(lavaSplash);
        newLavaSplash.transform.position = new Vector2(Random.Range(-4f, 4f), ypos);
        yield return new WaitForSeconds(4f);
        StartCoroutine(SpawnLavaSplash());
    }
}
