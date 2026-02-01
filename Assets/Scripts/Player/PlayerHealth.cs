using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Player Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthSlider;
    private int health;

    [Header("Health Flash Settings")]
    [Tooltip("Flash Color")]
    [SerializeField] Color colorToGoTo;

    [Tooltip("Flash Time")]
    [SerializeField] float flashTime;

    private Color startColor;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        healthSlider.value = 1f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            StoryHandler.GoNextScene();
        }
        StartCoroutine(FlashEffect());
        healthSlider.value = (float)health / (float)maxHealth;
    }
    IEnumerator FlashEffect()
    {
        spriteRenderer.color = colorToGoTo;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = startColor;
    }

}
