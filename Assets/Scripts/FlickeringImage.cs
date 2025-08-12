using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickeringImage : MonoBehaviour
{
    Material originalMaterial;
    Sprite originalSprite;
    [SerializeField] int imageInt;
    [SerializeField] List<Material> imgList = new List<Material>();
    [SerializeField] float timeBetweenFlickerSets, startDelay;
    Renderer thisRenderer;

    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        originalMaterial = thisRenderer.material;
        //originalSprite = ownImage.sprite;
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(Flicker());
    }
    IEnumerator Flicker()
    {
        int randomAmount = Random.Range(1, 6);
        float randomTime1 = Random.Range(timeBetweenFlickerSets - 2, timeBetweenFlickerSets + 2);
        yield return new WaitForSeconds(randomTime1);
        for(int i = 0; i < randomAmount; i++)
        {
            float randomTime2 = Random.Range(0.1f, 0.25f);
            float randomTime3 = Random.Range(0.1f, 0.25f);
            thisRenderer.material = imgList[imageInt];
            yield return new WaitForSeconds(randomTime2);
            thisRenderer.material = originalMaterial;
            yield return new WaitForSeconds(randomTime3);
        }
        StartCoroutine(Flicker());
    }

}
