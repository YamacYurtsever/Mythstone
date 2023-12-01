using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGem : MonoBehaviour
{
    private GemGenerator gemGenerator;
    Vector2[] lavaDirections;
    Vector2[] superDirections;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpecialGemDestroyer();
    }

    public void SpecialGemDestroyer()
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
        GetComponent<Gem>().DestroyIt();

        // Vertical
        if (transform.rotation.eulerAngles.z == 0)
        {
            float startY = gemGenerator.spawnHeight - gemGenerator.screenRowNumber * gemGenerator.spawnGapY;
            for (int i = 0; i <= gemGenerator.screenRowNumber; i++)
            {
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, startY));
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
                Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(startX, transform.position.y));
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
        GetComponent<Gem>().DestroyIt();

        foreach (Vector2 direction in lavaDirections)
        {
            Collider2D gemCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y) + direction);
            if (gemCollider != null)
            {
                if (gemCollider.tag == "DinoTooth" || gemCollider.tag == "Lava" || gemCollider.tag == "Super")
                    gemCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                else
                    gemCollider.GetComponent<Gem>().DestroyIt();
            }
        }
    }

    private void DestroySuper()
    {
        GetComponent<Gem>().DestroyIt();

        foreach (Vector2 direction in superDirections)
        {
            Vector2 currentPoint = transform.position;
            Vector2 adjacentPoint = currentPoint + direction;
            Collider2D adjacentCollider = Physics2D.OverlapPoint(adjacentPoint);
            if (adjacentCollider != null)
            {
                if (adjacentCollider.tag == "DinoTooth" || adjacentCollider.tag == "Lava" || adjacentCollider.tag == "Super")
                    adjacentCollider.GetComponent<SpecialGem>().SpecialGemDestroyer();
                else
                    adjacentCollider.GetComponent<Gem>().DestroyGroup();
            }
        }
    }

    private void Awake()
    {
        gemGenerator = GameObject.FindGameObjectWithTag("Gem Generator").GetComponent<GemGenerator>();

        lavaDirections = new Vector2[]
        {
            new Vector2(gemGenerator.spawnGapX, 0),
            new Vector2(gemGenerator.spawnGapX, gemGenerator.spawnGapY),
            new Vector2(gemGenerator.spawnGapX, -gemGenerator.spawnGapY),
            new Vector2(-gemGenerator.spawnGapX, 0),
            new Vector2(-gemGenerator.spawnGapX, gemGenerator.spawnGapY),
            new Vector2(-gemGenerator.spawnGapX, -gemGenerator.spawnGapY),
            new Vector2(0, gemGenerator.spawnGapY),
            new Vector2(0, -gemGenerator.spawnGapY)
        };

        superDirections = new Vector2[]
        {
            new Vector2(gemGenerator.spawnGapX, 0),
            new Vector2(-gemGenerator.spawnGapX, 0),
            new Vector2(0, gemGenerator.spawnGapY),
            new Vector2(0, -gemGenerator.spawnGapY)
        };
    }
}
