using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; set; }

    public int enemiesMerked = 0;
    public int time = 0;
    private float t = 0.0f;
    private bool gameOver = false;
    private bool bossMerked = false;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateScore()
    {
        enemiesMerked++;
    }
    
    public void BossMerked()
    {
        bossMerked=true;
    }

    public void GameOver()
    {
        if (bossMerked)
        {
            //do something like you win
        } else
        {
            //do something for dying
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check if game is over
        //if not update the time
        t += Time.deltaTime;
        if(t > 1.0f)
        {
            t = 0.0f;
            time++;
        }

    }
}
