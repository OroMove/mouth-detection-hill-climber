using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] groundPrefabs;  // Array to hold different ground prefabs
    public Transform groundParent;      // Parent object to hold instantiated ground
    public float[] levelDistances;      // Array for different level distances

    private int currentLevel = 0;
    private GameObject currentGround;

    void Start()
    {
        LoadLevel(currentLevel);  // Load first level
    }

    public void LoadLevel(int levelIndex)
    {
        if (currentGround != null)
        {
            Destroy(currentGround);  // Remove old ground
        }

        // Instantiate new ground
        currentGround = Instantiate(groundPrefabs[levelIndex], groundParent);
        currentGround.transform.position = Vector3.zero;

        // Set the level distance
        SetLevelDistance(levelDistances[levelIndex]);
    }

    void SetLevelDistance(float distance)
    {
        // Adjust ground size (optional, based on your level design)
        groundParent.localScale = new Vector3(distance, 1, 1);
    }

    public void NextLevel()
    {
        currentLevel = (currentLevel + 1) % groundPrefabs.Length;
        LoadLevel(currentLevel);
    }
}
