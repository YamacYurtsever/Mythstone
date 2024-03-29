using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jack : MonoBehaviour
{
    public int jackorder;

    private GemGenerator gemGenerator;

    private void Start()
    {
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
    }

    private void JackDestroyer()
    {
        Destroy(gameObject);
        GameObject jackGenerator = GameObject.FindGameObjectWithTag("Jack Generator");
        jackGenerator.GetComponent<JackGenerator>().jackCrashEventTrigger();
    }

    private void Update()
    {
        if (transform.position.y >= gemGenerator.spawnHeight)
            JackDestroyer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        JackDestroyer();
    }
}
