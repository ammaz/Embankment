using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{

    #region singleton

    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    //Phase Panel
    public GameObject BuildPhasePanel;
    public GameObject AttackPhasePanel;

    //Gameplay UI
    public GameObject gameplayUI;

    //Health Bar Canvas
    public Canvas HealthBarCanvas;

    //For Camera
    public Camera Camera;

    //Phase Check
    public bool BuildPhase;
    public bool AttackPhase;

    //Player Outpost
    public GameObject playerOutpost;
    //Enemy Outpost
    public GameObject enemyOutpost;

    //Victory Panel
    public GameObject VictoryPanel;
    public Text VictoryMoney;
    public Text VictoryOil;
    public Text VictoryOutpostHP;
    //Defeat Panel
    public GameObject DefeatPanel;
    public Text DefeatMoney;
    public Text DefeatOil;
    public Text DefeatOutpostHP;

    //Loading Screen Panel
    public Slider loadingSlider;
    public Text loadingProgressText;
    public GameObject loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayPhasePanel(BuildPhasePanel));
        BuildPhase = true;
        PhaseCheck();
    }

    // Update is called once per frame
    void Update()
    {
        PhaseCheck();
        GameOver();
    }

    public IEnumerator DisplayPhasePanel(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(4f);
        panel.SetActive(false);
    }

    public void GameOver()
    {
        if (playerOutpost == null)
        {
            Defeat();
        }
        else if (enemyOutpost == null)
        {
            Victory();
        }
        else if (enemyOutpost != null && playerOutpost != null && !AttackPhase && !BuildPhase)
        {
            Victory();
        }
        else
        {
            //Do Nothing
        }
    }

    public void Victory()
    {
        VictoryOil.text = "" + 100;
        VictoryMoney.text = "" + 200;
        VictoryOutpostHP.text = "" + playerOutpost.GetComponent<HealthManager>().currentHealth + "%";
        GameManager.instance.AddMoneyItemToInventory(200);
        GameManager.instance.AddOilItemToInventory(100);
        gameplayUI.SetActive(false);
        VictoryPanel.SetActive(true);
    }

    public void Defeat()
    {
        DefeatOil.text = "" + 5;
        DefeatMoney.text = "" + 10;
        DefeatOutpostHP.text = "" + enemyOutpost.GetComponent<HealthManager>().currentHealth + "%";
        GameManager.instance.AddMoneyItemToInventory(10);
        GameManager.instance.AddOilItemToInventory(5);
        gameplayUI.SetActive(false);
        DefeatPanel.SetActive(true);
    }

    public void PhaseCheck()
    {
        if (TimerController.instance.timeValue <= 0 && BuildPhase)
        {
            BuildPhase = false;
            AttackPhase = true;
            StartCoroutine(DisplayPhasePanel(AttackPhasePanel));
            TimerController.instance.timeValue = 123;
        }
        else if (TimerController.instance.timeValue <= 0 && AttackPhase)
        {
            BuildPhase = false;
            AttackPhase = false;
            GameOver();
        }
    }

    public void Retreat()
    {
        Defeat();
    }

    public void GotoHome()
    {
        gameplayUI.SetActive(false);
        VictoryPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(0));
    }

    public IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingSlider.value = 0;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;

        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            loadingSlider.value = progress;
            loadingProgressText.text = Math.Round((decimal)progress, 1) * 100 + "%";
            if (progress >= 0.9f)
            {
                loadingSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;

        }
    }
}
