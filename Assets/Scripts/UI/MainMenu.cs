using BitBenderGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            levelInfo.text = "Level 1\n\nLevel Info";
        }
        else if (levelIndex == 2)
        {
            levelInfo.text = "Level 2\n\nLevel Info";
        }
        else if (levelIndex == 3)
        {
            levelInfo.text = "Level 3\n\nLevel Info";
        }
        else if (levelIndex == 4)
        {
            levelInfo.text = "Level 4\n\nLevel Info";
        }
        else if (levelIndex == 5)
        {
            levelInfo.text = "Level 5\n\nLevel Info";
        }
        else
        {
            levelInfo.text = "";
        }
    }

    public IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Curent progress
            slider.value = progress;
            progressText.text = (int)progress * 100 + "%";

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
