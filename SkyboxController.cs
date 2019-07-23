using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script to smoothly transition between n different skyboxes

//Designed to be used in combination with the following shader:
//https://wiki.unity3d.com/index.php/SkyboxBlended
//+
//Any skybox/6 sided skybox package

//Add this script to a Unity GameObject
//Add your starting skybox to the lighting settings within Unity

public class SkyboxController : MonoBehaviour
{

    public Material[] skyArray;
    private int currentSkyIndex = 0;
    private bool transitionActive;
    private float skyboxBlendFactor = 0;
    public float skyboxDuration;
    public float skyboxBlendSpeed;

    void Start()
    {
        RenderSettings.skybox.SetFloat("_Blend", skyboxBlendFactor);

    }

    void Update()
    {
        //Check the game is still active
        if (GameControl.instance.gameOver)
        {
            CancelInvoke();
            transitionActive = false;
        }
        //Stops the blend going above the next transition
        if (transitionActive && skyboxBlendFactor <= 1)
        {
            skyboxBlendFactor += skyboxBlendSpeed * Time.deltaTime;
            RenderSettings.skybox.SetFloat("_Blend", skyboxBlendFactor);
        }

    }

    public void StartSkyTransitions()
    {
        currentSkyIndex = 0;
        //Start the repeating transition to automatically occur on a set time delay
        InvokeRepeating("SkyTransitioning", skyboxDuration, skyboxDuration);
    }

    private void SkyTransitioning()
    {

        RenderSettings.skybox = skyArray[currentSkyIndex];

        if (currentSkyIndex < skyArray.Length - 1)
        {
            currentSkyIndex += 1;
        }
        else
        {
            currentSkyIndex = 0;
        }
        skyboxBlendFactor = 0;

        transitionActive = true;
        //Sets the blend to zero for the new skybox
        RenderSettings.skybox.SetFloat("_Blend", skyboxBlendFactor);

    }
}
