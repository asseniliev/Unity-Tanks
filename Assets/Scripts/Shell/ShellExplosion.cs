using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;  // 
    public ParticleSystem m_ExplosionParticles;  // The particle effect for a shell explosion 
    public AudioSource m_ExplosionAudio;         // The audio source attached to the particle effect     
    public float m_MaxDamage = 100f;             // The max damabe that the shell can cause (it corresponds to a direct hit on the tank)     
    public float m_ExplosionForce = 1000f;       // The force that the explosion will do     
    public float m_MaxLifeTime = 2f;             // The shell will be destroyed after 2 seconds (in case it does not hit anything)      
    public float m_ExplosionRadius = 5f;         // The radius stating how far objects will be affected when the shell explodes     


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);  // Destroy function is initiated immediately after object's instantionation - so it will be distroyed after 2 seconds anyway
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)  // This method will be called when something bumps into the shell's trigger
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidBody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidBody)  // Check if the object affected by the OverlapSphere has a target body
                continue;

            targetRigidBody.AddExplosionForce(m_ExplosionForce, this.transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidBody.GetComponent<TankHealth>();
            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidBody.position);

            targetHealth.TakeDamage(damage);
        }

        m_ExplosionParticles.transform.parent = null; // we want to unparent the particles as they must play even after the shell is distroyed.

        m_ExplosionParticles.Play(); // Play the particle effects.
        m_ExplosionAudio.Play(); // Play the explosion audio.
         
        Destroy(m_ExplosionParticles.gameObject, t: m_ExplosionParticles.duration);

        Destroy(this.gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - this.transform.position;
        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;   // It's a number starting from 0 (near the center of explosion) and ending to 1, at the center

        float damage = relativeDistance * m_MaxDamage; // The closer to the center we are, the bigger the damage is

        damage = Mathf.Max(0f, damage); //We are making sure that the damage is not negative 

        return damage;
    }
}