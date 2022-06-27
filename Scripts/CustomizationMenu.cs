using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SaveNamespace;
using TMPro;

public class CustomizationMenu : MonoBehaviour
{
    public int redUnlockValue = 50;
    public int blueUnlockValue = 100;
    public int greenUnlockValue = 200;
    public GameObject menuObject;
    public GameObject tcText;

    SaveInfo saveInfo = new SaveInfo();

    //private vars
    //red UI
    private GameObject unlockObjectRed;
    private GameObject unlockButtonRed;
    private GameObject selectButtonRed;
    private GameObject selectedButtonRed;
    //blue UI
    private GameObject unlockObjectBlue;
    private GameObject unlockButtonBlue;
    private GameObject selectButtonBlue;
    private GameObject selectedButtonBlue;
    //green UI
    private GameObject unlockObjectGreen;
    private GameObject unlockButtonGreen;
    private GameObject selectButtonGreen;
    private GameObject selectedButtonGreen;

    // Start is called before the first frame update
    void Start()
    {
        loadData();

        //find UI elements
        //red UI
        unlockObjectRed = gameObject.transform.Find("UnlockRed").gameObject;
        unlockButtonRed = unlockObjectRed.transform.Find("RedUnlockButton").gameObject;
        selectButtonRed = unlockObjectRed.transform.Find("RedSelectButton").gameObject;
        selectedButtonRed = unlockObjectRed.transform.Find("RedEquipped").gameObject;

        //blue UI
        unlockObjectBlue = gameObject.transform.Find("UnlockBlue").gameObject;
        unlockButtonBlue = unlockObjectBlue.transform.Find("BlueUnlockButton").gameObject;
        selectButtonBlue = unlockObjectBlue.transform.Find("BlueSelectButton").gameObject;
        selectedButtonBlue = unlockObjectBlue.transform.Find("BlueEquipped").gameObject;

        //green UI
        unlockObjectGreen = gameObject.transform.Find("UnlockGreen").gameObject;
        unlockButtonGreen = unlockObjectGreen.transform.Find("GreenUnlockButton").gameObject;
        selectButtonGreen = unlockObjectGreen.transform.Find("GreenSelectButton").gameObject;
        selectedButtonGreen = unlockObjectGreen.transform.Find("GreenEquipped").gameObject;


        updateUnlocks();
        updateEquipped();
        tcText.GetComponent<TextMeshProUGUI>().text = TrophyUIOffset(saveInfo.trophy);
    }

    //UI DEBUG HELPER
    /*void Update() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            Debug.Log("It's over UI elements");
        }
        else
        {
            Debug.Log("It's NOT over UI elements");
        }
    }*/

    private void updateUnlocks() {
        if (saveInfo.trophy >= redUnlockValue) {
            unlockRed();
        }
        if (saveInfo.trophy >= blueUnlockValue) {
            unlockBlue();
        }
        if (saveInfo.trophy >= greenUnlockValue) {
            unlockGreen();
        }
    }

    private void updateEquipped() {
        //used for when the menu is reopened and needs to show the currently equipped color
        if (saveInfo.activeColor == "red") {
            selectRed();
        } 
        else if (saveInfo.activeColor == "blue") {
            selectBlue();
        }
        else if (saveInfo.activeColor == "green") {
            selectGreen();
        }
    }

    //UI unlock methods
    private void unlockRed() {
        unlockButtonRed.SetActive(false);
        selectButtonRed.SetActive(true);
    }

    private void unlockBlue() {
        unlockButtonBlue.SetActive(false);
        selectButtonBlue.SetActive(true);
    }

    private void unlockGreen() {
        unlockButtonGreen.SetActive(false);
        selectButtonGreen.SetActive(true);
    }

    public void selectRed() {
        if (unlockButtonRed.activeInHierarchy == false) {
            if (selectButtonRed.activeInHierarchy == true) {
                //update UI
                selectButtonRed.SetActive(false);
                selectedButtonRed.SetActive(true);
                unSelectBlue(false);
                unSelectGreen(false);

                saveInfo.activeColor = "red";
                saveData(saveInfo);
            }
        }
    }

    public void unSelectRed(bool BlockUpdateColor = true) {
        if (selectedButtonRed.activeInHierarchy == true) {
            //update UI
            selectedButtonRed.SetActive(false);
            selectButtonRed.SetActive(true);

            //should we NOT update player color? (usually this is true when called from another color select method)
            if (BlockUpdateColor == true) {
                saveInfo.activeColor = "default";
                saveData(saveInfo);
            }
        }
    }

    public void selectBlue() {
        Debug.Log("BLUE");
        if (unlockButtonBlue.activeInHierarchy == false) {
            if (selectButtonBlue.activeInHierarchy == true) {
                //update UI
                selectButtonBlue.SetActive(false);
                selectedButtonBlue.SetActive(true);
                unSelectRed(false);
                unSelectGreen(false);

                saveInfo.activeColor = "blue";
                saveData(saveInfo);
            }
        }
    }

    public void unSelectBlue(bool BlockUpdateColor = true) {
        if (selectedButtonBlue.activeInHierarchy == true) {
            //update UI
            selectedButtonBlue.SetActive(false);
            selectButtonBlue.SetActive(true);

            //should we update player color? (usually this is false when called from another color select method)
            if (BlockUpdateColor == true) {
                saveInfo.activeColor = "default";
                saveData(saveInfo);
            }
        }
    }

    public void selectGreen() {
        if (unlockButtonGreen.activeInHierarchy == false) {
            if (selectButtonGreen.activeInHierarchy == true) {
                //update UI
                selectButtonGreen.SetActive(false);
                selectedButtonGreen.SetActive(true);
                unSelectRed(false);
                unSelectBlue(false);

                saveInfo.activeColor = "green";
                saveData(saveInfo);
            }
        }
    }

    public void unSelectGreen(bool BlockUpdateColor = true) {
        if (selectedButtonGreen.activeInHierarchy == true) {
            //update UI
            selectedButtonGreen.SetActive(false);
            selectButtonGreen.SetActive(true);

            //should we update player color? (usually this is false when called from another color select method)
            if (BlockUpdateColor == true) {
                saveInfo.activeColor = "default";
                saveData(saveInfo);
            }
        }
    }

    private string TrophyUIOffset (int count) { //just visual adjustment
        if (count < 10) {
            //double space before number
            string offsetString = "  ";
            string countString = count.ToString();
            return offsetString + countString;
        }
        else if (count >= 10 && count < 100) {
            //single space before number
            string offsetString = " ";
            string addedString = offsetString + count.ToString();
            return addedString;
        }
        else {
            return count.ToString();
        }
    }

    public void saveData(SaveNamespace.SaveInfo saveClass) {
        string json = JsonUtility.ToJson(saveClass);
        File.WriteAllText(Application.dataPath + "/saveFile.json", json);
    }

    public void loadData() {
        string Readjson = File.ReadAllText(Application.dataPath + "/saveFile.json");
        saveInfo = JsonUtility.FromJson<SaveNamespace.SaveInfo>(Readjson);
    }

}
