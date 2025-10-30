using UnityEngine;

public class Boundary : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruits"))
        {
            Debug.Log("boundary hit");

            gameManager.LoseLife();
            Destroy(other.gameObject);
        }
    }
}
