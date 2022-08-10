using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace Environment
{
    public class WeatherSimulation : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float EmissionMultiplyer;
        [SerializeField] private float maxChangeWindDirectionCooldown = 5;

        private PostProcessVolume _postProcessingVolume;


        private float _changeWindDirectionCooldown;
        

        public Vector3 Wind
        {
            get
            {
                return _wind;
            }
            set
            {
                WindUpdate(value);
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
            _changeWindDirectionCooldown = maxChangeWindDirectionCooldown;
           
        }
        /// <summary>
        /// Smooth tranisition from 1 Wind state to another using Lerp (Post Proccesing included)
        /// </summary>
        /// <returns></returns>
        private IEnumerator WindTransition(Vector3 newWind)
        {
            Vignette vg;
            ChromaticAberration chab;
            _postProcessingVolume.profile.TryGetSettings(out vg);
            _postProcessingVolume.profile.TryGetSettings(out chab);
            var vel = _snowSystem.velocityOverLifetime;


            var startVI = vg.intensity.GetValue<float>();
            var startFog = RenderSettings.fogDensity;
            var startVS = vg.smoothness.GetValue<float>();
            var startC = chab.intensity.GetValue<float>();
            float targetVI, targetVS, targetC, targetFog, t = 0f;
            //End Value will be the value chosen below
            if (newWind.magnitude >= 11)
            {
                targetVI = 0.36f;
                targetVS = 0.25f;
                targetFog = 0.07f;
                targetC = 0.38f;
            }
            else if (newWind.magnitude >= 6)
            {
                targetVI = 0.24f;
                targetVS = 0.20f;
                targetFog = 0.001f;
                targetC = 0.15f;
            }
            else
            {
                targetVI = 0f;
                targetVS = 0f;
                targetFog = 0f;
                targetC = 0f;
            }
            //Smoothly change values
            while (t < 1f)
            {
               
                t += Time.deltaTime / 15;
                if (t > 1f)
                    t = 1f;
                //Assigining new PostProccesing Values
                chab.intensity.Override(Mathf.Lerp(startC, targetC, t));
                vg.intensity.Override(Mathf.Lerp(startVI, targetVI, t));
                vg.smoothness.Override(Mathf.Lerp(startVS, targetVS, t));
                RenderSettings.fogDensity = Mathf.Lerp(startFog, targetFog, t);

                //Assiging new Wind Values
                vel.x = newWind.x != 0 ? new ParticleSystem.MinMaxCurve(Vector3.Lerp(_wind, newWind, Mathf.Clamp(t * 3f, 0f,  1f)).x - 0.5f, Vector3.Lerp(_wind, newWind, Mathf.Clamp(t * 3f, 0f, 1f)).x + 0.5f) : new ParticleSystem.MinMaxCurve(-1, 1);
                vel.z = newWind.z != 0 ? new ParticleSystem.MinMaxCurve(Vector3.Lerp(_wind, newWind, Mathf.Clamp(t * 3f, 0f, 1f)).z - 0.5f, Vector3.Lerp(_wind, newWind, Mathf.Clamp(t * 3f, 0f, 1f)).z + 0.5f) : new ParticleSystem.MinMaxCurve(-1, 1);


                yield return null;
            }
            _wind = newWind;
            Debug.Log("___");
            yield return null;
        }

        private void Start()
        {
            _postProcessingVolume = GetComponent<PostProcessVolume>();
            _snowSystem = GetComponent<ParticleSystem>();
            GenerateRandomWind();
        }

        private void Update()
        {
            transform.position = player.position;
            _changeWindDirectionCooldown -= Time.deltaTime;
            if (_changeWindDirectionCooldown <= 0)
                GenerateRandomWind();


            Vignette vg;
            ChromaticAberration chab;
            _postProcessingVolume.profile.TryGetSettings(out vg);
            _postProcessingVolume.profile.TryGetSettings(out chab);

        }

        
        

        /// <summary>
        /// Updates Wind and everything related 
        /// </summary>
        private void WindUpdate(Vector3 newWind)
        {
            
            var emission = _snowSystem.emission;
            var main = _snowSystem.main;
            var collision = _snowSystem.collision;
            
            emission.rateOverTime = newWind.magnitude * newWind.magnitude * EmissionMultiplyer + 10;
            main.startLifetime = 100 - 4 * newWind.magnitude;
            collision.lifetimeLoss = newWind.magnitude > 7 ? 1 : 0.95f;
            Debug.Log(newWind.magnitude);
            //Start Coroutine
            StartCoroutine(WindTransition(newWind));

        }

        
    }
    
}
