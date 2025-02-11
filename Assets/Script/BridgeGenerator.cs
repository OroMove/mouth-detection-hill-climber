using UnityEngine;

public class RopeBridgeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject plankPrefab; // Assign BridgePlank prefab
    [SerializeField] private Transform leftAnchor; // Start point
    [SerializeField] private Transform rightAnchor; // End point
    [SerializeField] private int plankCount = 10; // Number of planks
    [SerializeField] private float plankSpacing = 0.8f; // Distance between planks

    private void Start()
    {
        GenerateBridge();
    }

    private void GenerateBridge()
    {
        Vector2 startPos = leftAnchor.position;
        Vector2 endPos = rightAnchor.position;
        Vector2 direction = (endPos - startPos).normalized; // Direction between anchors
        float totalLength = Vector2.Distance(startPos, endPos);
        float plankGap = totalLength / (plankCount - 1); // Even spacing

        GameObject previousPlank = leftAnchor.gameObject;

        for (int i = 0; i < plankCount; i++)
        {
            Vector2 spawnPos = startPos + (direction * plankGap * i);
            GameObject plank = Instantiate(plankPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = plank.GetComponent<Rigidbody2D>();
            HingeJoint2D joint = plank.GetComponent<HingeJoint2D>();

            if (previousPlank != null)
            {
                // Attach hinge joint to the previous plank
                joint.connectedBody = previousPlank.GetComponent<Rigidbody2D>();
            }

            previousPlank = plank;
        }

        // Attach the last plank to the right anchor
        previousPlank.GetComponent<HingeJoint2D>().connectedBody = rightAnchor.GetComponent<Rigidbody2D>();
    }
}
