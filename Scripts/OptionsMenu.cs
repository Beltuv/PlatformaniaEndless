using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CrazyModeButtonDisabled;
    public GameObject CrazyModeButtonEnabled;
    void Start()
    {
       LoadCrazyModeUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCrazyMode() {
        if (CrazyModeButtonDisabled.active == false) {
           //Make Disabled
           CrazyModeButtonDisabled.SetActive(true);
           CrazyModeButtonEnabled.SetActive(false);

           GameEffect.CrazyModeIsActive = false;
           GameEffect.ToggleCrazyMode = true;
       } else {
           //Make Enabled
           CrazyModeButtonDisabled.SetActive(false);
           CrazyModeButtonEnabled.SetActive(true);

           GameEffect.CrazyModeIsActive = true;
           GameEffect.ToggleCrazyMode = false;
       }
    }
    public void LoadCrazyModeUI() {
        if (GameEffect.CrazyModeIsActive == false) {
           //Make Disabled
           CrazyModeButtonDisabled.SetActive(true);
           CrazyModeButtonEnabled.SetActive(false);
       } else {
           //Make Enabled
           CrazyModeButtonDisabled.SetActive(false);
           CrazyModeButtonEnabled.SetActive(true);
       }
    }
}
