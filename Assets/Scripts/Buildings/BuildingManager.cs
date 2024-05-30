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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null)
            {
                Instantiate(activeBuildingType.prefab, UtilClass.GetMouseWorldPosition(), Quaternion.identity);
            }
        }
    }
}
