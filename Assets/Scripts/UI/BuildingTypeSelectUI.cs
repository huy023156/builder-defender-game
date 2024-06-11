using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    public static BuildingTypeSelectUI Instance;

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    [SerializeField]
    private Sprite arrowSprite;
    private RectTransform arrowRectTransform;

    private RectTransform btnTemplate;
    private BuildingTypeListSO buildingTypeList;

    private BuildingTypeSO activeBuildingType;

    private Dictionary<BuildingTypeSO, RectTransform> buildingTypeButtonDictionary;

    private void Awake()
    {
        Instance = this;

        buildingTypeButtonDictionary = new Dictionary<BuildingTypeSO, RectTransform>();

        btnTemplate = transform.Find("btnTemplate").GetComponent<RectTransform>();
        btnTemplate.gameObject.SetActive(false);

        buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypeListSO");

        int n = 0;
        int left = 140;

        arrowRectTransform = Instantiate(btnTemplate, transform);
        arrowRectTransform.gameObject.SetActive(true);
        arrowRectTransform.transform.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowRectTransform.anchoredPosition = new Vector2(left * n, arrowRectTransform.anchoredPosition.y);

        arrowRectTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach (BuildingTypeSO buildingType in buildingTypeButtonDictionary.Keys)
            {
                buildingTypeButtonDictionary[buildingType].Find("selected").gameObject.SetActive(false);
            }
            arrowRectTransform.Find("selected").gameObject.SetActive(true);
            activeBuildingType = null;
            OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
        });

        arrowRectTransform.SetParent(transform);
        n++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            RectTransform btn = Instantiate(btnTemplate, transform);

            btn.gameObject.SetActive(true);

            btn.transform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;
            btn.anchoredPosition = new Vector2(left * n, btn.anchoredPosition.y);
            btn.Find("selected").gameObject.SetActive(false);

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                foreach (BuildingTypeSO buildingType in buildingTypeButtonDictionary.Keys)
                {
                    buildingTypeButtonDictionary[buildingType].Find("selected").gameObject.SetActive(false);
                    btn.Find("selected").gameObject.SetActive(true);
                }
                arrowRectTransform.Find("selected").gameObject.SetActive(false);

                btn.Find("selected").gameObject.SetActive(true);
                activeBuildingType = buildingType;
                OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
            });

            btn.GetComponent<MouseEnterExitEvents>().OnMouseEnter += (object sender, EventArgs e) => {
                TooltipUI.Instance.Show(buildingType.name, 10);
            };
            btn.GetComponent<MouseEnterExitEvents>().OnMouseExit += (object sender, EventArgs e) => {
                TooltipUI.Instance.Hide();
            };

            buildingTypeButtonDictionary[buildingType] = btn;
            btn.SetParent(transform);
            n++;
        }

        activeBuildingType = null;
        arrowRectTransform.Find("selected").gameObject.SetActive(true);
    }
}
