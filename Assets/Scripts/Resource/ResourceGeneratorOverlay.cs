using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    private ResourceGenerator resourceGenerator;

    private float timer;
    private float timerMax;

    private void Awake()
    {
        resourceGenerator = GetComponentInParent<ResourceGenerator>();
    }

    private void Start()
    {
        timerMax = resourceGenerator.GetTimerMax();

        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGenerator.GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData.resourceType.sprite;

        if (timerMax == Mathf.Infinity)
        {
            transform.Find("text").GetComponent<TextMeshPro>().SetText("0.0");
        }
        else
        {
            transform.Find("text").GetComponent<TextMeshPro>().SetText(timerMax.ToString("0.0"));
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = timerMax;
        }

        Transform bar = transform.Find("bar");

        bar.localScale = new Vector3(1 - GetTimerNormalized(), bar.localScale.y, bar.localScale.z);
    }

    private float GetTimerNormalized()
    {
        return timer / timerMax;
    }
}
