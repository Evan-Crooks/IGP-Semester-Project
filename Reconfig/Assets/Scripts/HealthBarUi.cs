using UnityEngine;

public class HealthBarUi : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _HealthBarForeGroundImage;
    public void UpdateHealthBar(PlayerHealth playerHealth)
    {

        _HealthBarForeGroundImage.fillAmount = playerHealth.health/playerHealth.maxHealth;
    }
}
