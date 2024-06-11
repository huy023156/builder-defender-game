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
            if (CanSpawnBuilding(activeBuildingType, UtilClass.GetMouseWorldPosition(), out string errorMessage) && ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray) )
            {
                ResourceManager.Instance.SpendResources(activeBuildingType);
                Instantiate(activeBuildingType.prefab, UtilClass.GetMouseWorldPosition(), Quaternion.identity);
            } else
            {
                TooltipUI.Instance.Show(errorMessage, 2);
            }
        }

    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        // check area clear
        float distanceCheck = 4f;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, distanceCheck);

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<Building>() != null || collider2D.GetComponent<ResourceNode>() != null)
            {
                errorMessage = "Area is not clear";
                return false;
            }
        }

        // is near other building?
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        bool isNearOtherBuilding = false;

        foreach (Collider2D collider in collider2DArray)
        {
            if (collider.GetComponent<Building>() != null)
            {
                isNearOtherBuilding = true;
                break;
            }
        }

        if (!isNearOtherBuilding)
        {
            errorMessage = "To far from other buildings";
            return false;
        }

        // is near other building of the same type
        distanceCheck = 10f;

        collider2DArray = Physics2D.OverlapCircleAll(position, distanceCheck);

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<BuildingTypeHolder>() != null)
            {
                if (collider2D.GetComponent<BuildingTypeHolder>().buildingType == buildingType)
                {
                    errorMessage = "To close to a building of the same type";
                    return false;
                }
            }
        }
        
        errorMessage = null;
        return true;
    }
}
