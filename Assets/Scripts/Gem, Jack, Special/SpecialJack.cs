using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialJack : MonoBehaviour
{
    private GemGenerator gemGenerator;
    private Vector2 collisionPosition;
    private Collider2D collisionCol;
    private GameObject gemStorage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionCol = collision;
        collisionPosition = collision.transform.position;

        // Double of Same Type
        if (gameObject.tag == "DinoTooth" && collision.tag == "DinoTooth")
        {
            if (transform.rotation.eulerAngles.z == 0 && collision.transform.rotation.eulerAngles.z == 0)
                DoubleVerticalDinoTooth();
            else if (transform.rotation.eulerAngles.z == 90 && collision.transform.rotation.eulerAngles.z == 90)
                DoubleHorizontalDinoTooth();
        }
        else if (gameObject.tag == "Lava" && collision.tag == "Lava")
            DoubleLava();
        else if (gameObject.tag == "Super" && collision.tag == "Super")
            DoubleSuper();
        
        SpecialJackDestroyer();
    }

    public void SpecialJackDestroyer()
    {
        if (gameObject.tag == "DinoTooth")
            DestroyDinoTooth();
        else if (gameObject.tag == "Lava")
            DestroyLava();
        else if (gameObject.tag == "Super")
            DestroySuper();
    }

    private void DestroyDinoTooth()
    {
        // Vertical
        if (transform.rotation.eulerAngles.z == 0)
        {
            float startY = gemGenerator.spawnHeight - gemGenerator.screenRowNumber * gemGenerator.spawnGapY;
            for (int i = 0; i <= gemGenerator.screenRowNumber; i++)
            {
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(collisionPosition.x, startY));
                if (gemCollider != null)
                {
                    if (gemCollider.tag == "DinoTooth" || gemCollider.tag == "Lava" || gemCollider.tag == "Super")
                        gemCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                    else
                        gemCollider.GetComponent<Gem>().DestroyIt();
                }
                startY += gemGenerator.spawnGapY;
            }
        }

        // Horizontal
        else
        {
            float startX = 0 - ((gemGenerator.columnNumber / 2) * gemGenerator.spawnGapX);
            for (int i = 0; i < gemGenerator.columnNumber; i++)
            {
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(startX, collisionPosition.y));
                if (gemCollider != null)
                {
                    if (gemCollider.tag == "DinoTooth" || gemCollider.tag == "Lava" || gemCollider.tag == "Super")
                        gemCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                    else
                        gemCollider.GetComponent<Gem>().DestroyIt();
                }
                startX += gemGenerator.spawnGapX;
            }
        }
    }

    private void DestroyLava()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(collisionPosition.x + j * gemGenerator.spawnGapX, collisionPosition.y + i * gemGenerator.spawnGapY));
                if (gemCollider != null)
                {
                    if (gemCollider.tag == "DinoTooth" || gemCollider.tag == "Lava" || gemCollider.tag == "Super")
                        gemCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                    else
                        gemCollider.GetComponent<Gem>().DestroyIt();
                }
            }
        }
    }

    private void DestroySuper()
    {
        if (collisionCol.tag == "DinoTooth" || collisionCol.tag == "Lava" || collisionCol.tag == "Super")
            collisionCol.GetComponent<SpecialGem>().SpecialGemDestroyer();
        else
            collisionCol.GetComponent<Gem>().DestroyGroup();
    }

    private void DoubleVerticalDinoTooth()
    {
        for (int j = -1; j < 2; j++)
        {
            float startY = gemGenerator.spawnHeight - gemGenerator.screenRowNumber * gemGenerator.spawnGapY;
            for (int i = 0; i < gemGenerator.screenRowNumber; i++)
            {
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(collisionPosition.x + j * gemGenerator.spawnGapX, startY));
                if (gemCollider != null)
                {
                    if (gemCollider.tag == "DinoTooth" || gemCollider.tag == "Lava" || gemCollider.tag == "Super")
                        gemCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                    else
                        gemCollider.GetComponent<Gem>().DestroyIt();
                }
            startY += gemGenerator.spawnGapY;
            }
        }
    }

    private void DoubleHorizontalDinoTooth()
    {
        float limitY = gemGenerator.spawnHeight - gemGenerator.screenRowNumber * gemGenerator.spawnGapY;

        for (int j = -1; j < 2; j++)
        {
            float startX = 0 - ((gemGenerator.columnNumber / 2) * gemGenerator.spawnGapX);
            for (int i = 0; i < gemGenerator.columnNumber; i++)
            {
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(startX, collisionPosition.y + j * gemGenerator.spawnGapY));
                if (gemCollider != null && collisionPosition.y + j >= limitY)
                {
                    if (gemCollider.tag == "DinoTooth" || gemCollider.tag == "Lava" || gemCollider.tag == "Super")
                        gemCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                    else
                        gemCollider.GetComponent<Gem>().DestroyIt();
                }
                startX += gemGenerator.spawnGapX;
            }
        }
    }

    private void DoubleLava()
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(collisionPosition.x + j * gemGenerator.spawnGapX, collisionPosition.y + i * gemGenerator.spawnGapY));
                if (gemCollider != null)
                {
                    if (gemCollider.tag == "DinoTooth" || gemCollider.tag == "Lava" || gemCollider.tag == "Super")
                        gemCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                    else
                        gemCollider.GetComponent<Gem>().DestroyIt();
                }
            }
        }
    }

    private void DoubleSuper()
    {
        for (int i = 0; i < gemStorage.transform.childCount; i++)
        {
            gemStorage.transform.GetChild(i).GetComponent<Gem>().DestroyIt();
        }
    }

    private void Awake()
    {
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();
        gemStorage = GameObject.FindGameObjectWithTag("Gem Storage");
    }
}
