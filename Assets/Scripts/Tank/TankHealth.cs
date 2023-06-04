using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f; // The initial health of the player
    public Slider m_Slider;               // Reference to the health slider
    public Image m_FillImage;             // The Image component of the Fill gameobject (inside the Slider's hierarchy)          
    public Color m_FullHealthColor = Color.green;  // The color of full-health - green
    public Color m_ZeroHealthColor = Color.red;    // The color of no health - red
    public GameObject m_ExplosionPrefab;  // The explosion particle effect 
    public bool simulateDamage;
    
    private AudioSource m_ExplosionAudio;         // The explosion radio source 
    private ParticleSystem m_ExplosionParticles;  // The particle system (attached to the explosion prefab)
    private float m_CurrentHealth;                // The current health
    private bool m_Dead;                          // Checks if the player is dead


    private void Awake()
    {
        //We immediately instantiate the tank explosion prefab but will deactiave it until the time comes
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();  // We instantiate the explosion game object and get the particle system
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }


    public void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;
        SetHealthUI();

        if (m_CurrentHealth <= 0 && !m_Dead)
        {
            OnDeath();
        }
    }

    private void Update()
    {
        SimlateTakeDamage();
    }


    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        m_Dead = true;
        m_ExplosionParticles.transform.position = this.transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        this.gameObject.SetActive(false);
    }

    private void SimlateTakeDamage()
    {
        if(simulateDamage)
        {
            TakeDamage(10);
            simulateDamage = false;
        }
    }
}