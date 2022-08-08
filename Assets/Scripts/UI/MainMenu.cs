using BitBenderGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    #region singleton

    public static MainMenu instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    private int levelIndex;
    public Text levelInfo;

    //For Loading Slider
    public Slider slider;
    public Text progressText;

    public GameObject loadingScreen;
    public GameObject levelScreen;

    //For Oil and Money Slider and Count
    public Text MoneyText;
    public Slider MoneySlider;
    private int MoneyCount;

    public Text OilText;
    public Slider OilSlider;
    private int OilCount;

    // Start is called before the first frame update
    void Start()
    {
        levelIndex = 0;
        MoneyCount = 0;
        OilCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGame()
    {
        if (levelIndex == 0)
        {
            levelInfo.text = "Please select a level";
        }

        else if (levelIndex == 2 || levelIndex == 3 || levelIndex == 4 || levelIndex == 5)
        {
            levelInfo.text = "Coming Soon!";
        }

        else
        {
            levelScreen.SetActive(false);
            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsynchronously(levelIndex));
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        levelIndex = sceneIndex;
        if (levelIndex == 1)
        {
            levelInfo.text = "Level 1\n\nDesert Island";
        }
        else if (levelIndex == 2)
        {
            levelInfo.text = "Level 2\n\nVolcanic Island";
        }
        else if (levelIndex == 3)
        {
            levelInfo.text = "Level 3\n\nSnow Island";
        }
        else if (levelIndex == 4)
        {
            levelInfo.text = "Level 4\n\nForest Island";
        }
        else if (levelIndex == 5)
        {
            levelInfo.text = "Level 5\n\nBeach Island";
        }
        else
        {
            levelInfo.text = "";
        }
    }

    public IEnumerator LoadAsynchronously(int sceneIndex)
    {
        slider.value = 0;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;

        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            slider.value = progress;
            progressText.text = Math.Round((decimal)progress, 1) * 100 + "%";
            if (progress >= 0.9f)
            {
                slider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void EnableOrDisableCamera(bool check)
    {
        GameObject.Find("Main Camera").GetComponent<MobileTouchCamera>().enabled = check;
    }
    
    //For counting number of Oil or Money in our inventory
    public void CountMoneyOrOil()
    {
        //Resetting Count
        MoneyCount = 0;
        OilCount = 0;

        //For Oil and Money Slider and Count
        for (int a = 0; a < Inventory.instance.inventoryItemList.Count; a++)
        {
            if (Inventory.instance.inventoryItemList[a].name == "Oil")
            {
                OilCount++;
            }
            else if (Inventory.instance.inventoryItemList[a].name == "Money")
            {
                MoneyCount++;
            }
            else
            {
                continue;
            }
        }

        OilText.text = "" + OilCount;
        OilSlider.value = OilCount;

        MoneyText.text = "" + MoneyCount;
        MoneySlider.value = MoneyCount;
    }
}
