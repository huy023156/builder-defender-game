using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceAmountUI : MonoBehaviour
{
    private RectTransform iconTemplate;
    private Dictionary<ResourceTypeSO, RectTransform> resourceTypeTransformDictionary;

    ResourceTypeListSO resourceTypeList;

    private void Awake()
    {
        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, RectTransform>();

        iconTemplate = transform.Find("iconTemplate").GetComponent<RectTransform>();
        iconTemplate.gameObject.SetActive(false);

        resourceTypeList = Resources.Load<ResourceTypeListSO>("ResourceTypeListSO");

        int n = 0;
        int right = 180;


        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            RectTransform rectTranform = Instantiate(iconTemplate, transform);

            rectTranform.gameObject.SetActive(true);

            rectTranform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            rectTranform.anchoredPosition = new Vector2(- right * n, rectTranform.anchoredPosition.y);

            resourceTypeTransformDictionary[resourceType] = rectTranform;
            rectTranform.SetParent(transform);
            n++;
        }
    }

    private void Update()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeTransformDictionary.Keys)
        {
            resourceTypeTransformDictionary[resourceType].transform.Find("text").GetComponent<TextMeshProUGUI>().SetText(ResourceManager.Instance.GetResourceAmount(resourceType).ToString());
        }
    }
}
