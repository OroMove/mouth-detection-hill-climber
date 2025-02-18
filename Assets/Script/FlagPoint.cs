using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPoint : MonoBehaviour
{
    public GameObject levelCompletePanel; // Assign in Inspector
    public Button nextLevelButton; // Assign in Inspector

    private void Start()
    {
        levelCompletePanel.SetActive(false); // Hide panel at start
        nextLevelButton.onClick.AddListener(LoadNextLevel); // Attach button click event
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure only the player triggers this
        {
            Debug.Log("Player reached the finish!");
            levelCompletePanel.SetActive(true); // Show level completion panel
            UnlockNewLevel();
        }
    }

    void UnlockNewLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex >= PlayerPrefs.GetInt("ReachedIndex", 0))
        {
            PlayerPrefs.SetInt("ReachedIndex", currentIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Last level reached.");
        }
    }
}
