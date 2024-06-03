using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool showing;

    private void Start()
    {
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();


        BuildingTypeSelectUI.Instance.OnActiveBuildingTypeChanged += BuildingTypeSelectUI_OnActiveBuildingTypeChanged;

        spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }

    private void Update()
    {
        if (showing) 
            transform.position = UtilClass.GetMouseWorldPosition();
    }

    private void BuildingTypeSelectUI_OnActiveBuildingTypeChanged(object sender, BuildingTypeSelectUI.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.activeBuildingType == null)
        {
            Hide();
        }

        if (e.activeBuildingType != null)
        {
            spriteRenderer.sprite = e.activeBuildingType.sprite;
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        showing = true;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        showing = false;
    }
}
