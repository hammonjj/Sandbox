using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarImage;

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }

    public void UpdateHealth(float fraction)
    {
        HealthBarImage.fillAmount = fraction;
    }
}
