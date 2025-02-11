using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour
{
    private int moveDirection;
    public float fuelLevel = 1f; // Assuming max is 1
    public float fuelConsumptionRate;

    public Image fuelBar;
    public Gradient fuelGradient; // Gradient for color change

    public float torqueForce = 150f;
    public float carTorque = 300f;
    public float horizontalInput;
    public Rigidbody2D frontWheel;
    public Rigidbody2D rearWheel;
    public Rigidbody2D carBody;

    void Update()
    {
        CaptureInput();
        fuelBar.fillAmount = fuelLevel;
        fuelBar.color = fuelGradient.Evaluate(fuelLevel); // Set color based on fuel level
    }

    private void FixedUpdate()
    {
        if (fuelLevel > 0)
        {
            ApplyMovement();
            ApplyMobileMovement();
        }
        ManageFuelConsumption();
    }

    void CaptureInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void ApplyMovement()
    {
        float torque = -horizontalInput * torqueForce * Time.fixedDeltaTime;

        frontWheel.AddTorque(torque * 0.8f);
        rearWheel.AddTorque(torque * 0.8f);

        carBody.AddTorque(-torque * 0.5f);
    }

    void ApplyMobileMovement()
    {
        float torque = moveDirection * torqueForce * Time.fixedDeltaTime;

        frontWheel.AddTorque(torque * 0.8f);
        rearWheel.AddTorque(torque * 0.8f);

        carBody.AddTorque(-torque * 0.5f);
    }

    void ManageFuelConsumption()
    {
        fuelLevel -= fuelConsumptionRate * Mathf.Abs(horizontalInput) * Time.fixedDeltaTime;
        fuelLevel -= fuelConsumptionRate * Mathf.Abs(moveDirection) * Time.deltaTime;
        fuelLevel = Mathf.Clamp01(fuelLevel); // Keep it between 0 and 1
    }

    public void SetMobileInput(int direction)
    {
        moveDirection = direction;
    }

    public GameObject flag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == flag)
        {
            FindObjectOfType<GameManager>().EndGameInstantly();
        }
    }
}
