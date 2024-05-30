using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private BuildingTypeSO buildingType;

    private float generatorTimer;
    private float generatorTimerMax;

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        generatorTimerMax = buildingType.resourceGeneratorData.generatorTimerMax;
    }

    private void Update()
    {
        generatorTimer -= Time.deltaTime;

        if (generatorTimer < 0)
        {
            ResourceManager.Instance.AddResource(buildingType.resourceGeneratorData.resourceType, 1);   
            generatorTimer = generatorTimerMax;
        }

    }
}
