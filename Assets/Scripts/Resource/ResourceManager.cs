using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceTypeSO, int> resourceTypeAmountDictionary = new Dictionary<ResourceTypeSO, int>();

    ResourceTypeListSO resourceTypeList;

    private void Awake()
    {
        Instance = this;

        resourceTypeList = Resources.Load<ResourceTypeListSO>("ResourceTypeListSO");

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceTypeAmountDictionary[resourceType] = 100;
        }
    }

    private void Update()
    {
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceTypeAmountDictionary[resourceType] += amount;
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceTypeAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] constructionResourceCostArray) 
    {
        foreach (ResourceAmount resourceAmount in constructionResourceCostArray) {
            if (resourceAmount.amount > resourceTypeAmountDictionary[resourceAmount.resourceType])
            {
                return false;
            }
        }

        return true;
    }

    public void SpendResources(BuildingTypeSO buildingType)
    {
        foreach (ResourceAmount resourceAmount in buildingType.constructionResourceCostArray)
        {
            if (resourceAmount.amount < resourceTypeAmountDictionary[resourceAmount.resourceType])
            {
                resourceTypeAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
            }
        }
    }
}
