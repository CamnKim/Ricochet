using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text centerText;
    public GameObject exitLight;
    public GameObject door;
    public Material winMat;
    public int score = 0;
    public int maxScore = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();
        if (score >= maxScore) Win();
    }

    void Win()
    {
        exitLight.GetComponent<Light>().color = Color.green;
        exitLight.GetComponent<MeshRenderer>().material = winMat;
        door.SetActive(false);
        
    }

    void updateScore()
    {
        scoreText.text = "Score: " + score.ToString() + "/" + maxScore.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        door.SetActive(true);
        centerText.text = "You Win!";
        GameObject.FindGameObjectWithTag("Player").GetComponent<CursorLock>().cLock = false;
    }
}
