using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Player Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private Slider healthSlider;
    private float health;

    [Header("Health Flash Settings")]
    [Tooltip("Flash Color")]
    [SerializeField] Color colorToGoTo;

    [Tooltip("Flash Time")]
    [SerializeField] float flashTime;

    [SerializeField] AudioClip deathAudio;

    private Color startColor;
    private SpriteRenderer spriteRenderer;
    private CameraFollow cameraFollow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        healthSlider.value = 1f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        AudioArray audioArray = GetComponent<AudioArray>();
        health -= damage;
        cameraFollow.Screenshake();
        if (health <= 0)
        {
            health = 0;
            GetComponent<AudioSource>().PlayOneShot(deathAudio);
            Time.timeScale = 0f;
            StartCoroutine(DeathSequence());
        }
        else
        {
            audioArray.PlayRandomOneShot();
        }
        StartCoroutine(FlashEffect());
        healthSlider.value = health / maxHealth;
    }
    IEnumerator FlashEffect()
    {
        spriteRenderer.color = colorToGoTo;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = startColor;
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSecondsRealtime(2.1f);
        Time.timeScale = 1f;
        StoryHandler.GoNextScene();
    }

}
