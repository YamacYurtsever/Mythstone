using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [System.Serializable]
    public struct storyScene
    {
        public Sprite background;
        public List<GameObject> objects;
    }
    public List<storyScene> storyScenes;
    public SpriteRenderer storySprite;
    public Camera mainCamera;
    public SceneLoader sceneLoader;
    public Transform objectsHolder;

    private int sceneIndex = 0;
    private int objectIndex = 0;
    private bool transitioning = false;

    private void Awake()
    {
        storySprite.sprite = storyScenes[0].background;
        
        AdjustStorySpriteSize();
    }

    private void AdjustStorySpriteSize()
    {
        float ysizeCamera = mainCamera.orthographicSize;

        float ysizeSprite = storySprite.sprite.rect.height / storySprite.sprite.pixelsPerUnit;

        float scaletoFit = (2 * ysizeCamera) / ysizeSprite;
        storySprite.transform.localScale *= new Vector2(scaletoFit, scaletoFit);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !transitioning)
        {
            NextObject();
        }
    }

    SpriteRenderer sceneObjectSr;
    private void NextObject()
    {
        if (objectIndex == storyScenes[sceneIndex].objects.Count)
        {
            NextPanel();
            return;
        }

        GameObject sceneObject = Instantiate(storyScenes[sceneIndex].objects[objectIndex]);

        // Set Scene Object Position
        float posx = sceneObject.GetComponent<SceneObjectPosition>().posx;
        float posy = sceneObject.GetComponent<SceneObjectPosition>().posy;

        float mainCameraHeight = mainCamera.orthographicSize * 2;
        float mainCameraWidth = mainCameraHeight * mainCamera.aspect;

        Vector2 pos = new Vector2((posx - 0.5f) * mainCameraWidth, (posy - 0.5f) * mainCameraHeight);
        sceneObject.transform.position = pos;

        // Start Scene Object Fade In
        transitioning = true;
        sceneObjectSr = sceneObject.GetComponent<SpriteRenderer>();
        Color oldcolor = sceneObjectSr.color;
        sceneObjectSr.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, 0);
        StartCoroutine(FadeInSceneObject());

        // not setting position right now
        sceneObject.transform.parent = objectsHolder;
        objectIndex++;
    }

    // Transition Fade Panel: Main Method and Constants
    [Header("Transition Fade, Scene Object")]
    public float fadeTimeSceneObject = 0.3f;

    IEnumerator FadeInSceneObject()
    {
        float increase = Time.deltaTime * (1 / fadeTimeSceneObject);
        Color oldcolor = sceneObjectSr.color;
        sceneObjectSr.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, oldcolor.a + increase);
        yield return new WaitForEndOfFrame();

        if (sceneObjectSr.color.a > 0.97)
        {
            transitioning = false;
            sceneObjectSr.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, 1);
        }
        else
        {
            StartCoroutine(FadeInSceneObject());
        }
    }

    private void NextPanel()
    {
        sceneIndex++;
        if (sceneIndex >= storyScenes.Count)
        {
            // Transition Over
            LoadGameScene();
        }
        else TransitionFadeInOutPanel();
    }

    private void TransitionBasic()
    {
        storySprite.sprite = storyScenes[sceneIndex].background;
    }

    // Transition Fade Panel: Main Method and Constants
    [Header("Transition Fade, Panel")]
    public float fadeTimePanel = 0.4f;
    public float waitBetweenFadePanel = 0.5f;

    private void TransitionFadeInOutPanel()
    {
        transitioning = true;
        StartCoroutine(FadeOutPanel());
    }

    // Transition Fade Panel: Details


    IEnumerator FadeOutPanel()
    {
        float decrease = Time.deltaTime * (1 / fadeTimePanel);
        Color oldcolor = storySprite.color;
        storySprite.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, oldcolor.a - decrease);
        yield return new WaitForEndOfFrame();

        // Also fade out every scene object
        for (int i = 0; i < objectsHolder.childCount; i++)
        {
            SpriteRenderer sceneObjectSpriteRenderer = objectsHolder.GetChild(i).GetComponent<SpriteRenderer>();
            oldcolor = sceneObjectSpriteRenderer.color;
            sceneObjectSpriteRenderer.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, oldcolor.a - decrease);
        }

        if (storySprite.color.a < 0.03)
        {
            StartCoroutine(WaitBeforeFadeInPanel());
            storySprite.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, 0);

            // Also fade out every scene object
            for (int i = 0; i < objectsHolder.childCount; i++)
            {
                SpriteRenderer sceneObjectSpriteRenderer = objectsHolder.GetChild(i).GetComponent<SpriteRenderer>();
                oldcolor = sceneObjectSpriteRenderer.color;
                sceneObjectSpriteRenderer.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, 0);
            }
        }
        else
        {
            StartCoroutine(FadeOutPanel());
        }
    }

    IEnumerator WaitBeforeFadeInPanel()
    {
        yield return new WaitForSeconds(waitBetweenFadePanel);

        // New Story Scene Shown 
        storySprite.sprite = storyScenes[sceneIndex].background;
        
        // Some settings when initializing new story scene
        objectIndex = 0;
        for (int i = 0; i < objectsHolder.childCount; i++)
        {
            Destroy(objectsHolder.GetChild(i).gameObject);
        }

        StartCoroutine(FadeInPanel());
    }

    IEnumerator FadeInPanel()
    {
        float increase = Time.deltaTime * (1 / fadeTimePanel);
        Color oldcolor = storySprite.color;
        storySprite.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, oldcolor.a + increase);
        yield return new WaitForEndOfFrame();

        if (storySprite.color.a > 0.97)
        {
            transitioning = false;
            storySprite.color = new Color(oldcolor.r, oldcolor.g, oldcolor.b, 1);
        }
        else
        {
            StartCoroutine(FadeInPanel());
        }
    }


    private void LoadGameScene()
    {
        Debug.Log("Load Game Scene");
        sceneLoader.LoadStartScene();
    }
}
