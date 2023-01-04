using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourBase
{
    public Image HealthBarImage;

    public void UpdateHealth(float fraction)
    {
        HealthBarImage.fillAmount = fraction;
    }
}
