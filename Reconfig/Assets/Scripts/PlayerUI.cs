using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    private PlayerHealth playerHealth;
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        if(player == null)
        {
            return;
        }
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
        if(slider.value == 0)
        {
            fillImage.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null)
        {
            return;
        }
        //update health based on player health
        UpdateHealthBar(playerHealth.health, playerHealth.maxHealth);
        //Update Score based on Score Manager
        scoreText.text = $"Score: {ScoreManager.instance.enemiesMerked}";
        //Update Time based on Score Manager
        timeText.text = $"Time: {ScoreManager.instance.time}";
    }
}
