using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplayController : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Transform heartsParent; 
    private List<GameObject> hearts = new List<GameObject>(); 

    public void SetLives(int lives)
    {
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        for (int i = 0; i < lives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsParent);
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * -120, 0); // shift between lifes
            hearts.Add(heart);
        }
    }
}
