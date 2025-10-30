
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    public float Speed = 20f, maxTorque = 6;
    int pointvalue = 5;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;
    public GameObject soundEffect;
    private float gravityModifier = 1f;
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(),
        RandomTorque(), ForceMode.Impulse);
        gameManager = GameObject.Find("GameManager")
            .GetComponent<GameManager>();

    }
    Vector3 RandomForce()
    {
        return Vector3.up * Speed;

    }
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (!coll.CompareTag("Blade")) return;
        else
        {
            if (gameManager.isGameActive)
            {
                if (gameObject.CompareTag("Bomb"))
                {
                    Destroy(gameObject);
                    Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
                    AudioSource Sound = Instantiate(soundEffect, transform.position, Quaternion.identity).GetComponent<AudioSource>();
                    Sound.Play();
                    Destroy(Sound.gameObject, Sound.clip.length);
                    gameManager.GameOver();
                }
                else
                {
                    Destroy(gameObject);
                    Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
                    AudioSource fruitSound = Instantiate(soundEffect, transform.position, Quaternion.identity).GetComponent<AudioSource>();
                    fruitSound.Play();
                    Destroy(fruitSound.gameObject, fruitSound.clip.length);
                    gameManager.UpdateScore(pointvalue);
                }
            }
        }
    }
}
