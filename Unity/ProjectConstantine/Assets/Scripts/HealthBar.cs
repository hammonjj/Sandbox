using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarImage;

    public void UpdateHealth(float fraction)
    {
        HealthBarImage.fillAmount = fraction;
    }
}
