using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public CarController car;

    public Text coinCounterText;
    public Text distanceText;
    public Text highestDistanceText;
    public Text finalDistanceText; // New UI text for Game Over screen

    public int totalCoins;
    public Transform playerTransform;
    private Vector2 startingPosition;

    private float currentDistance;
    private float highestDistance;

    async void Start()
    {
        //await InitializeUGS();
        //await SignIn();
        //await LoadHighestDistance();

        startingPosition = playerTransform.position;
        UpdateUI();
    }

    void Update()
    {
        CheckGameOver();
        CalculateDistanceTravelled();
        UpdateUI();
    }

    private void CheckGameOver()
    {
        if (car.fuelLevel <= 0)
        {
            FindObjectOfType<GameManager>().EndGameInstantly();
        }
    }

    public void EndGameInstantly()
    {
        gameOverScreen.SetActive(true); // Immediately show Game Over panel

        // Display current and highest distance on Game Over screen
        finalDistanceText.text = "Distance: " + currentDistance.ToString("F0") + " m";
        highestDistanceText.text = "Highest Distance: " + highestDistance.ToString("F0") + " m";

        SaveHighestDistance(); // Save the highest distance immediately
        Time.timeScale = 0f; // Instantly pause the game
    }



    private IEnumerator DisplayGameOverScreen()
    {
        yield return new WaitForSeconds(1.0f);
        gameOverScreen.SetActive(true);

        // Display current and highest distance on Game Over screen
        finalDistanceText.text = "Distance: " + currentDistance.ToString("F0") + " m";
        highestDistanceText.text = "Highest Distance: " + highestDistance.ToString("F0") + " m";

        _ = SaveHighestDistance(); // Call async method without awaiting
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    private void UpdateUI()
    {
        coinCounterText.text = totalCoins.ToString();
        distanceText.text = currentDistance.ToString("F0") + " m";
        highestDistanceText.text = "Highest Distance: " + highestDistance.ToString("F0") + " m";
    }

    private void CalculateDistanceTravelled()
    {
        currentDistance = Mathf.Max(0, playerTransform.position.x - startingPosition.x);
    }

    private async Task InitializeUGS()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized) return;
        await UnityServices.InitializeAsync();
    }

    private async Task SignIn()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        }
    }

    private async Task LoadHighestDistance()
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "HighestDistance" });
            if (data.ContainsKey("HighestDistance"))
            {
                highestDistance = float.Parse(data["HighestDistance"].ToString());
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load highest distance: " + e.Message);
        }
    }

    private async Task SaveHighestDistance()
    {
        if (currentDistance > highestDistance)
        {
            highestDistance = currentDistance;
            var saveData = new Dictionary<string, object> { { "HighestDistance", highestDistance } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(saveData);
            Debug.Log("Highest Distance Saved: " + highestDistance);
        }
    }
}
