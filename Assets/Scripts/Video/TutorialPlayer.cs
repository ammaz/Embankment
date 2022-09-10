using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialPlayer : MonoBehaviour
{
    //public GameObject videoPanel;

    public double time;
    public double currentTime;
    public GameObject videoPanel;

    // Start is called before the first frame update
    void Start()
    {
        time = gameObject.GetComponent<VideoPlayer>().clip.length;
        time -= 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = gameObject.GetComponent<VideoPlayer>().time;
        if (currentTime >= time)
        {
            videoPanel.SetActive(false);
        }
    }
}
