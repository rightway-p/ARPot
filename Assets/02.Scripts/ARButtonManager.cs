using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARButtonManager : MonoBehaviour
{

    public Image targetPanel;

    private Camera arCam;

    private List<GameObject> targetObjList = new List<GameObject>();

    private GameObject selectedTarget;

    private Vector2 camCenterPos;

    private Color tpColor;



    void Start()
    {
        arCam = Camera.main;
        camCenterPos = new Vector2(Screen.width/2, Screen.height/2);
        tpColor = targetPanel.color;

    }
    void Update()
    {
        TargetSearch();
    }

#region BUTTON

    public void OnInfoClick()
    {
        
    }

    public void OnCaptureClick()
    {

    }

    public void OnGrowthClick()
    {

    }
#endregion

#region TARGETING
    public void OnVuTargetFound(GameObject newTarget)
    {
        if(!targetObjList.Contains(newTarget))
        {
            targetObjList.Add(newTarget);
        }
    }
    
    public void OnVuTargetLost(GameObject oldTarget)
    {
        if(targetObjList.Contains(oldTarget))
        {
            targetObjList.Remove(oldTarget);
        }
    }



    void TargetSearch()
    {
        GameObject nearestTarget = SearchTargetInArea();
        SelectedTargetUpdate(nearestTarget);
    }

    GameObject SearchTargetInArea()
    {
        GameObject nearestTarget = null;

        float minDist = 987654321;
        foreach(var obj in targetObjList)
        {
            Vector3 objScreenPos = arCam.WorldToScreenPoint(obj.transform.position);
            Vector2 objectXYPos = new Vector2(objScreenPos.x, objScreenPos.y);

            float curDist = Vector2.Distance(camCenterPos, objectXYPos);

            if(minDist > curDist)
            {
                nearestTarget = obj;
                minDist = curDist;
            }
        }
        return nearestTarget;
    }

    void SelectedTargetUpdate(GameObject nearestTarget)
    {
        if(nearestTarget)
        {
            if(!this.selectedTarget)
            {
                this.selectedTarget = nearestTarget;
                SetSelectedTargetColor(this.selectedTarget, true);
            }
            else if(this.selectedTarget.name != nearestTarget.name)
            {
                SetSelectedTargetColor(this.selectedTarget, false);
                this.selectedTarget = nearestTarget;
                SetSelectedTargetColor(this.selectedTarget, true);
            }
        }
        else if(this.selectedTarget)
        {
            SetSelectedTargetColor(this.selectedTarget, false);
            this.selectedTarget = null;
        }
    }

    void SetSelectedTargetColor(GameObject obj, bool isSelected)
    {
        if(isSelected)
        {
            Debug.Log("a");
            obj.transform.Find("Canvas").transform.Find("Panel").GetComponent<Image>().color = new Color(this.tpColor.r, this.tpColor.g, this.tpColor.b, 0.8f);
        }
        else
        {
            obj.transform.Find("Canvas").transform.Find("Panel").GetComponent<Image>().color = new Color(this.tpColor.r, this.tpColor.g, this.tpColor.b, this.tpColor.a);
        }
    }
#endregion


#region GROWTHSLIDER

    public void OnGrowthSliderChanged(GameObject slider)
    {
        if(selectedTarget)
        {
            float value = slider.GetComponent<Slider>().value;
            Image[] images = selectedTarget.transform.Find("Canvas").transform.Find("GrowingImgGroup").GetComponentsInChildren<Image>();
            GrowthImageTransparency(images, value);
        }
    }

    void GrowthImageTransparency(Image[] images, float value)
    {
        Color imgColor;

        if(value > 0 && value <= 1)
        {
            imgColor = images[1].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, 1.0f - value);
            images[1].color = imgColor;
        }

        if(value > 0.5 && value <= 1)
        {
            imgColor = images[2].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, (value - 0.5f)*2.0f);
            images[2].color = imgColor;
        }

        if(value > 1 && value <= 1.5)
        {
            imgColor = images[2].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, 3.0f - (2.0f * value));
            images[2].color = imgColor;
        }

        if(value > 1 && value <= 2)
        {
            imgColor = images[3].color;
            imgColor = new Color(imgColor.r, imgColor.g, imgColor.b, value - 1);
            images[3].color = imgColor;
        }
    }
#endregion  
}