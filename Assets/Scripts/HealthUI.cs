using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image heartImage;
    public Sprite heart3;
    public Sprite heart2;
    public Sprite heart1;
    public Sprite heart0;

    public void UpdateHealth(int currentHealth)
    {
        switch (currentHealth)
        {
            case 3:
                heartImage.sprite = heart3;
                break;
            case 2:
                heartImage.sprite = heart2;
                break;
            case 1:
                heartImage.sprite = heart1;
                break;
            default:
                heartImage.sprite = heart0;
                break;
        }
    }
}
