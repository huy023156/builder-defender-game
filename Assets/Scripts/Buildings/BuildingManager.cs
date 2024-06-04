using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        Instance = this;
        activeBuildingType = null;
    }

    private void Start()
    {
        BuildingTypeSelectUI.Instance.OnActiveBuildingTypeChanged += BuildingTypeSelectUI_OnActiveBuildingTypeChanged;
    }

    private void BuildingTypeSelectUI_OnActiveBuildingTypeChanged(object sender, BuildingTypeSelectUI.OnActiveBuildingTypeChangedEventArgs e)
    {
        activeBuildingType = e.activeBuildingType;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && activeBuildingType != null)
        {
            if (IsAreaCleared())
            {
                if (IsNearOtherBuilding())
                {
                    if (!IsNearOtherBuildingOfTheSameType())
                    {
                        if (activeBuildingType != null)
                        {
                            if (ResourceManager.Instance.CanAfford(activeBuildingType))
                            {
                                ResourceManager.Instance.SpendResources(activeBuildingType);
                                Instantiate(activeBuildingType.prefab, UtilClass.GetMouseWorldPosition(), Quaternion.identity);
                            }
                            else
                            {
                                // cant afford
                                Debug.Log("cant afford");
                            }
                        }
                    }
                    else
                    {
                        // too near to a building of the same type
                        Debug.Log("too near to a building of the same type");

                    }
                }
                else
                {
                    // not near any building
                    Debug.Log("not near any building");
                }
            }
            else
            {
                // area is not clear
                Debug.Log("area is not clear");
            }
        }
    }

    private bool IsAreaCleared()
    {
        float distanceCheck = 4f;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(UtilClass.GetMouseWorldPosition(), distanceCheck);

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<Building>() != null || collider2D.GetComponent<ResourceNode>() != null)
            {
                return false;
            }
        }


        return true;
    }

    private bool IsNearOtherBuilding()
    {
        if (activeBuildingType == null) return false;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(UtilClass.GetMouseWorldPosition(), activeBuildingType.minConstructionRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            if (collider.GetComponent<Building>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsNearOtherBuildingOfTheSameType()
    {
        float distanceCheck = 10f;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(UtilClass.GetMouseWorldPosition(), distanceCheck);

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<BuildingTypeHolder>() != null)
            {
                if (collider2D.GetComponent<BuildingTypeHolder>().buildingType == activeBuildingType)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
