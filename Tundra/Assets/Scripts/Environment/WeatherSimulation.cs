using UnityEngine;

namespace Environment
{
    public class WeatherSimulation : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float EmissionMultiplyer;
        [SerializeField] private float maxChangeWindDirectionCooldown = 5;
        
        private float changeWindDirectionCooldown;
        
        public Vector3 Wind
        {
            get
            {
                return _wind;
            }
            set
            {
                _wind = value;
                WindUpdate();
            }
        }
        
        private ParticleSystem _snowSystem;
        private Vector3 _wind;
        
        /// <summary>
        /// Generates random wind direction and resets the cooldown timer
        /// </summary>
        private void GenerateRandomWind()
        {
            Wind = Vector3.ClampMagnitude(new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)), Random.Range(0, 16));
            changeWindDirectionCooldown = maxChangeWindDirectionCooldown;
        }
        
        private void Start()
        {
            _snowSystem = GetComponent<ParticleSystem>();
            GenerateRandomWind();
        }

        private void Update()
        {
            transform.position = player.position;
            changeWindDirectionCooldown -= Time.deltaTime;
            if (changeWindDirectionCooldown <= 0)
                GenerateRandomWind();
        }

        /// <summary>
        /// Updates snowflakes
        /// </summary>
        private void WindUpdate()
        {
            var vel = _snowSystem.velocityOverLifetime;
            var emission = _snowSystem.emission;
            var main = _snowSystem.main;
            var collision = _snowSystem.collision;
            vel.x = Wind.x != 0 ? new ParticleSystem.MinMaxCurve(Wind.x - 0.5f, Wind.x + 0.5f) : new ParticleSystem.MinMaxCurve(-1, 1);
            vel.z = Wind.z != 0 ? new ParticleSystem.MinMaxCurve(Wind.z - 0.5f, Wind.z + 0.5f) : new ParticleSystem.MinMaxCurve(-1, 1);
            emission.rateOverTime = Wind.magnitude * Wind.magnitude * EmissionMultiplyer + 10;
            main.startLifetime = 100 - 4 * Wind.magnitude;
            collision.lifetimeLoss = Wind.magnitude > 7 ? 1 : 0.95f;
        }
    }
}
