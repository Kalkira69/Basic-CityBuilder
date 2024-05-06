using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingPlacement : MonoBehaviour
{
    //Variables
    private bool currentPlacing;
    private bool currentDestroying;

    private BuildingPreset curBuildingPreset;

    private float indicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;

    public GameObject placementPointer;
    public GameObject destroyPointer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();

        if(Time.time - lastUpdateTime > indicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
            curIndicatorPos = Selector.instance.GetCurTilePosition();

            if (currentPlacing)
                placementPointer.transform.position = curIndicatorPos;
            else if (currentDestroying)
                destroyPointer.transform.position = curIndicatorPos;
        }
        // called when left mouse button is pressed
        if (Input.GetMouseButtonDown(0) && currentPlacing)
            PlaceBuidling();
        else if (Input.GetMouseButtonDown(0) && currentDestroying)
            Destroy();

    }

    //selects building to be placed when click on UI
    public void BeginNewBuildingPlacement (BuildingPreset preset)
    {
        //check resource

        currentPlacing = true;
        curBuildingPreset = preset;
        placementPointer.SetActive(true);
        placementPointer.transform.position = new Vector3(0, -99, 0);

    }
    //used when building placed or escape is pressed
    void CancelBuildingPlacement()
    {
        currentPlacing = false;
        placementPointer.SetActive(false);
    }
    //toggle destroy on & off
    public void ToggleDestroy()
    {
        currentDestroying = !currentDestroying;
        destroyPointer.SetActive(currentDestroying);
        destroyPointer.transform.position = new Vector3(0, -99, 0);
    }

    void PlaceBuidling()
    {
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curIndicatorPos, Quaternion.identity);
        City.instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());

        CancelBuildingPlacement();

    }
    //delete current selected building
    void Destroy()
    {
        Building buildingToDestroy = City.instance.buildings.Find(x => x.transform.position == curIndicatorPos);

        if(buildingToDestroy != null)
        {
            City.instance.OnRemoveBuilding(buildingToDestroy);

        }

    }

}
