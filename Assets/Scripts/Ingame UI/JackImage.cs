using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackImage : MonoBehaviour
{
    // this way of adding jack images to jackImages public list of images in jackgenerator doesnt work if the UI This is attached to is inactive at start of game.

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
