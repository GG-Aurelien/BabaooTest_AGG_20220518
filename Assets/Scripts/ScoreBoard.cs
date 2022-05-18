using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    private Text score;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SuccessTime")) { score.text = "MAX : " + PlayerPrefs.GetFloat("SuccessTime"); }
        else score.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
