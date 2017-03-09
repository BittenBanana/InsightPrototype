using UnityEngine;

public class ParticleSeekOptimized : MonoBehaviour {

    public Transform target;
    public float force = 10.0f;

    new ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;

    ParticleSystem.MainModule particleSystemMainModule;

    // Use this for initialization
    void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystemMainModule = particleSystem.main; 
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if(particles == null || particles.Length < particleSystemMainModule.maxParticles)
        {
            particles = new ParticleSystem.Particle[particleSystemMainModule.maxParticles];
        }
        float forceDeltaTime = force * Time.deltaTime;

        Vector3 targetTransformPosition;

        switch(particleSystemMainModule.simulationSpace)
        {
            case ParticleSystemSimulationSpace.Local:
                {

                    targetTransformPosition = transform.InverseTransformPoint(target.position);
                }
                break;
            case ParticleSystemSimulationSpace.Custom:
                {

                    targetTransformPosition = particleSystemMainModule.customSimulationSpace.TransformPoint(target.position);
                }
                break;
            case ParticleSystemSimulationSpace.World:
                {

                    targetTransformPosition = target.position;
                }
                break;
            default:
                {
                    throw new System.NotSupportedException(
                        string.Format("Unsupported similation space '{0}'.",
                        System.Enum.GetName(typeof(ParticleSystemSimulationSpace), particleSystemMainModule.simulationSpace)));
                }

        }

        for (int i =0; i < particles.Length; i++)
        {
            Vector3 directionToTarget = Vector3.Normalize(targetTransformPosition - particles[i].position);

            Vector3 seekForce = directionToTarget * forceDeltaTime;

            particles[i].velocity += seekForce;
        }

        particleSystem.SetParticles(particles, particles.Length);
	}
}
