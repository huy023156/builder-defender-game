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
            resourceTypeAmountDictionary[resourceType] = 0;
        }
    }

    private void Update()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceTypeAmountDictionary[resourceType] += amount;
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceTypeAmountDictionary[resourceType];
    }
}
