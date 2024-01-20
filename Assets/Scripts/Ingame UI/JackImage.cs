using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackImage : MonoBehaviour
{
    public Image currentImage, comingImage;
    private GameObject jackGenerator;
    // Start is called before the first frame update
    void Start()
    {
        jackGenerator = GameObject.FindGameObjectWithTag("Jack Generator");
        jackGenerator.GetComponent<JackGenerator>().jackImages.Clear();
        jackGenerator.GetComponent<JackGenerator>().jackImages.Add(currentImage);
        jackGenerator.GetComponent<JackGenerator>().jackImages.Add(comingImage);
    }
}
