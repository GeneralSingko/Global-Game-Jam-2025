using UnityEngine;

public class PersistentBGM : MonoBehaviour
{
    private static PersistentBGM instance;

    private void Awake()
    {
        // Check if there's already an instance of this object
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Preserve this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate objects
        }
    }
}
