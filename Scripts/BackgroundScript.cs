using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothTime = 0.3f;
    public float HeightFactor = 0.2f;
    private Vector3 velocity = Vector3.zero;

    //Endless Background Generation
    public GameObject GrassBackground;
    public Transform GrassParent;
    public GameObject WinterBackground;
    public Transform WinterParent;
    public GameObject FallBackground;
    public Transform FallParent;
    public GameObject ForestBackground;
    public Transform ForestParent;
    public float BackgroundMoveDistance;
    float LeftSpawnOffset;
    float RightSpawnOffset;
    float SizeOfBG;
    List<GameObject> GrassBGs = new List<GameObject>();
    List<GameObject> WinterBGs = new List<GameObject>();
    List<GameObject> FallBGs = new List<GameObject>();
    List<GameObject> ForestBGs = new List<GameObject>();
    Vector3 lastLeftPosition;
    Vector3 lastRightPosition;

    Color32 VisibleColor = new Color32(255, 255, 255, 255);
    Color32 InvisibleColor = new Color32(0,0,0,0);

    void Start() {
        SizeOfBG = GrassBackground.GetComponent<SpriteRenderer>().bounds.size.x;
        RightSpawnOffset = GrassBackground.transform.position.x + SizeOfBG;
        LeftSpawnOffset = GrassBackground.transform.position.x - SizeOfBG;
        GameObject GrassBackgroundOverlapCopy = Instantiate(GrassBackground, GrassParent);
        GameObject WinterBackgroundOverlapCopy = Instantiate(WinterBackground, WinterParent);
        GameObject FallBackgroundOverlapCopy = Instantiate(FallBackground, FallParent);
        GameObject ForestBackgroundOverlapCopy = Instantiate(ForestBackground, ForestParent);
        GrassBGs.Add(GrassBackgroundOverlapCopy);
        WinterBGs.Add(WinterBackgroundOverlapCopy);
        FallBGs.Add(FallBackgroundOverlapCopy);
        ForestBGs.Add(ForestBackgroundOverlapCopy);
        lastLeftPosition = GrassBackgroundOverlapCopy.transform.position;
        lastRightPosition = GrassBackgroundOverlapCopy.transform.position;
        SetGrassBackgrounds();

        AddRightBackground();
        AddRightBackground();
        AddRightBackground();
        AddLeftBackground();
        AddLeftBackground();
        AddLeftBackground();
    }

    // Update is called once per frame
    void Update()
    {
        float newHeight = (playerTransform.position.y - transform.position.y) * HeightFactor;
        Vector3 targetPos = new Vector3 (transform.position.x, transform.position.y + newHeight, playerTransform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        if (Vector3.Distance(playerTransform.position, lastRightPosition) < BackgroundMoveDistance) {
            AddRightBackground();
            DestroyLeft();
        }
        if (Vector3.Distance(playerTransform.position, lastLeftPosition) < BackgroundMoveDistance) {
            AddLeftBackground();
            DestroyRight();
        }
    }

    void AddRightBackground() { //extends background to the right
        //Grass
        GameObject GrassCopy = Instantiate(GrassBackground, GrassParent);
        GameObject WinterCopy = Instantiate(WinterBackground, WinterParent);
        GameObject FallCopy = Instantiate(FallBackground, FallParent);
        GameObject ForestCopy = Instantiate(ForestBackground, ForestParent);

        GrassCopy.transform.position = new Vector3(RightSpawnOffset, GrassBackground.transform.position.y, GrassBackground.transform.position.z);
        WinterCopy.transform.position = new Vector3(RightSpawnOffset, WinterBackground.transform.position.y, WinterBackground.transform.position.z);
        FallCopy.transform.position = new Vector3(RightSpawnOffset, FallBackground.transform.position.y, FallBackground.transform.position.z);
        ForestCopy.transform.position = new Vector3(RightSpawnOffset, ForestBackground.transform.position.y, ForestBackground.transform.position.z);

        GrassBGs.Add(GrassCopy);
        WinterBGs.Add(WinterCopy);
        FallBGs.Add(FallCopy);
        ForestBGs.Add(ForestCopy);

        lastRightPosition = GrassCopy.transform.position;
        RightSpawnOffset = RightSpawnOffset + SizeOfBG;
    }
    
    void AddLeftBackground () {
        GameObject GrassCopy = Instantiate(GrassBackground, GrassParent);
        GameObject WinterCopy = Instantiate(WinterBackground, WinterParent);
        GameObject FallCopy = Instantiate(FallBackground, FallParent);
        GameObject ForestCopy = Instantiate(ForestBackground, ForestParent);

        GrassCopy.transform.position = new Vector3(LeftSpawnOffset, GrassBackground.transform.position.y, GrassBackground.transform.position.z);
        WinterCopy.transform.position = new Vector3(LeftSpawnOffset, WinterBackground.transform.position.y, WinterBackground.transform.position.z);
        FallCopy.transform.position = new Vector3(LeftSpawnOffset, FallBackground.transform.position.y, FallBackground.transform.position.z);
        ForestCopy.transform.position = new Vector3(LeftSpawnOffset, ForestBackground.transform.position.y, ForestBackground.transform.position.z);

        GrassBGs.Insert(0, GrassCopy);
        WinterBGs.Insert(0, WinterCopy);
        FallBGs.Insert(0, FallCopy);
        ForestBGs.Insert(0, ForestCopy);

        lastLeftPosition = GrassCopy.transform.position;
        LeftSpawnOffset = LeftSpawnOffset - SizeOfBG;
    }

    void DestroyLeft () {
        GameObject GrassObjectToDestroy = GrassBGs[0];
        GameObject WinterObjectToDestroy = WinterBGs[0];
        GameObject FallObjectToDestroy = FallBGs[0];
        GameObject ForestObjectToDestroy = ForestBGs[0];
        GrassBGs.RemoveAt(0);
        WinterBGs.RemoveAt(0);
        FallBGs.RemoveAt(0);
        ForestBGs.RemoveAt(0);
        Destroy(GrassObjectToDestroy);
        Destroy(WinterObjectToDestroy);
        Destroy(FallObjectToDestroy);
        Destroy(ForestObjectToDestroy);
        lastLeftPosition = GrassBGs[0].transform.position; //this should be the new far BG after removing the old one
        LeftSpawnOffset = LeftSpawnOffset + SizeOfBG;
    }
    void DestroyRight () {
        GameObject GrassObjectToDestroy = GrassBGs[GrassBGs.Count - 1];
        GameObject WinterObjectToDestroy = WinterBGs[WinterBGs.Count - 1];
        GameObject FallObjectToDestroy = FallBGs[FallBGs.Count - 1];
        GameObject ForestObjectToDestroy = ForestBGs[ForestBGs.Count - 1];
        GrassBGs.RemoveAt(GrassBGs.Count - 1);
        WinterBGs.RemoveAt(WinterBGs.Count - 1);
        FallBGs.RemoveAt(FallBGs.Count - 1);
        ForestBGs.RemoveAt(ForestBGs.Count - 1);
        Destroy(GrassObjectToDestroy);
        Destroy(WinterObjectToDestroy);
        Destroy(FallObjectToDestroy);
        Destroy(ForestObjectToDestroy);
        lastRightPosition = GrassBGs[GrassBGs.Count - 1].transform.position; //this should be the new far BG after removing the old one
        RightSpawnOffset = RightSpawnOffset - SizeOfBG;
    }

    //Background Biome updaters
    public void SetWinterBackgrounds () {
        foreach (Transform child in WinterParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = VisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 1f);
            }
        }
        foreach (Transform child in GrassParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in FallParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in ForestParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
    }
    public void SetGrassBackgrounds () {
        foreach (Transform child in WinterParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in GrassParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = VisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 1f);
            }
        }
        foreach (Transform child in FallParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in ForestParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
    }
    public void SetFallBackgrounds () {
        foreach (Transform child in WinterParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in GrassParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in FallParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = VisibleColor;
            Debug.Log(child.gameObject.GetComponent<SpriteRenderer>());
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 1f);
            }
        }
        foreach (Transform child in ForestParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
    }
    public void SetForestBackgrounds () {
        foreach (Transform child in WinterParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in GrassParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in FallParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = InvisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
        foreach (Transform child in ForestParent) {
            child.gameObject.GetComponent<SpriteRenderer>().color = VisibleColor;
            foreach (Transform extension in child) {
                extension.gameObject.GetComponent<SpriteRenderer>().color = new Color(extension.gameObject.GetComponent<SpriteRenderer>().color.r, extension.gameObject.GetComponent<SpriteRenderer>().color.g, extension.gameObject.GetComponent<SpriteRenderer>().color.b, 1f);
            }
        }
    }
}
