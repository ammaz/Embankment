using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //Phase Panel
    public GameObject BuildPhasePanel;
    public GameObject AttackPhasePanel;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(displayPhasePanel(BuildPhasePanel));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator displayPhasePanel(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(4f);
        panel.SetActive(false);
    }

}
