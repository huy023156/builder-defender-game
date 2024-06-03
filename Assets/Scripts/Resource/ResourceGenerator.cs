using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private BuildingTypeSO buildingType;

    private float timer;
    private float timerMax;

    private void Awake()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        timerMax = GetGeneratorTimerMax();
        Debug.Log(timerMax);
    }

    private void Update()
    {
        if (GetEfficiency() > 0)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                ResourceManager.Instance.AddResource(buildingType.resourceGeneratorData.resourceType, 1);
                timer = timerMax;
            }

            Debug.Log(GetEfficiency());
        }
    }

    public float GetEfficiency()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buildingType.resourceGeneratorData.resourceDetectionRadius);

        int detected = 0;

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<ResourceNode>())
            {
                detected++;
            }
        }

        float efficiency = (float)Mathf.Clamp(detected, 0, buildingType.resourceGeneratorData.maxResourceAmount) / buildingType.resourceGeneratorData.maxResourceAmount;
    
        return efficiency;
    }

    private float GetGeneratorTimerMax()
    {
        float efficiency = GetEfficiency();

        float generatorTimerMax = 1 / (efficiency * (1 / buildingType.resourceGeneratorData.timerMax));

        return generatorTimerMax;
    }
}
