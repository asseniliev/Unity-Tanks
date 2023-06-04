using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       // The player number
    public Rigidbody m_Shell;            // Reference to the shall we shall instantiate 
    public Transform m_FireTransform;    // The place where the shall will instantiate
    public Slider m_AimSlider;           // The yellow arrow slider reference
    public AudioSource m_ShootingAudio;  // Ref to the second audio source that is going to play the tank effects (shooting in this case)
    public AudioClip m_ChargingClip;     // The shell charging audio clip
    public AudioClip m_FireClip;         // The fire audio clip
    public float m_MinLaunchForce = 15f;   // Value of the min launch force (the same value is set to the yellow arrow slider)
    public float m_MaxLaunchForce = 30f;   // Value of the max launch force (the same value is set to the yellow arrow slider)
    public float m_MaxChargeTime = 0.75f;  // How long does it take to get from min launch force to max lounch force

    
    private string m_FireButton;          // Which button to press in order to fire
    private float m_CurrentLaunchForce;   // How much have we charged the shot so far (i.e. the current launch force)
    private float m_ChargeSpeed;          // The charge speed (the speed to go from minLaunchForce to maxLaunchForce)
    private bool m_Fired;                 // Did we fire or not yet


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;

        // Set the m_AimSlider min and max values equal to m_MinLaunchForce and m_MaxLaunchForce
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        // Adjust the min and max values of the aim slider to the min and max values of the launch force
        m_AimSlider.minValue = m_MinLaunchForce; 
        m_AimSlider.maxValue = m_MaxLaunchForce;

    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;
        if(m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)  // The max charged is reached
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(m_FireButton))   //The fire button was first pressed
        {
            m_Fired = false;           // At this moment we know we did not fire yet
            m_CurrentLaunchForce = m_MinLaunchForce;  // At this moment the change force is to its minimum
            m_ShootingAudio.clip = m_ChargingClip;  // Set the clip to charging clip
            m_ShootingAudio.Play();      // Play the charging clip
        }
        else if(Input.GetButton(m_FireButton) && !m_Fired)  // The fire button is held and we did not fire yet
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime; // Increase the charge force
            m_AimSlider.value = m_CurrentLaunchForce; // Update the length of the yellow arrow
        }
        else if(Input.GetButtonUp(m_FireButton) && !m_Fired) // The fire button was released and we did not fire yet
        {
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        m_ShootingAudio.clip = m_FireClip;  //Assigning a new clip will stop the audio source from playing the current clip
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}