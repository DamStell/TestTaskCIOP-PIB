using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarItemChecker : MonoBehaviour
{
    [SerializeField] Transform[] avatarTargets;
    [SerializeField] string nextSceneName;
    [SerializeField] ScreenFader screenFader; 

    void Update()
    {
        if (CheckAllItemsEquipped())
        {
            // use darkening effect to load scene
            screenFader.FadeOutAndLoadScene(nextSceneName);
        }
    }

    bool CheckAllItemsEquipped()
    {
        foreach (Transform target in avatarTargets)
        {
            if (target.childCount == 0) 
            {
                return false;
            }
        }
        return true; 
    }
}
