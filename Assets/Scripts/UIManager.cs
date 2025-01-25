using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    //calls singleton pattern
    public static UIManager UIManagerInstance;

    //UI elements
    [SerializeField] private GameObject singleFireImage, rapidFireImage;

    //Fire Mode UI
    GameObject currentActiveImage;
    private void Awake()
    {
        if (UIManagerInstance == null)
        {
            UIManagerInstance = this;
        } else if (UIManagerInstance != this)
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        //sets default currentActiveImage
        currentActiveImage = singleFireImage;
    }

    public void ChangeBulletIcon(int fireMode)
    {
        currentActiveImage.SetActive(false); //turns off active fire mode to swap it out with active one
        switch (fireMode)
        {
            case 1:
                currentActiveImage = singleFireImage;
                currentActiveImage.SetActive(true);
                break;
            case 2:
                currentActiveImage = rapidFireImage;
                rapidFireImage.SetActive(true);
                break;
        }
        
    }
}
