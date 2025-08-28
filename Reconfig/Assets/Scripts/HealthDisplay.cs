using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;
    //public Image[] hearts;

    public PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null)
        {
            health = 0;
            return;
        }
        else
        {
            health = playerHealth.health;
        }
        Debug.Log("Player health " + health);
        
        // for (int i = 0; i < hearts.Length; i++)
        // {

        //     if (i < health)
        //     {
        //         hearts[i].color = Color.red;
        //     }
        //     else
        //     {
        //         hearts[i].color = Color.gray;
        //     }
        // }
    }
}
