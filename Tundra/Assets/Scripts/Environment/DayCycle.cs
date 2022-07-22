using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    /// <summary>
    /// Is used to loop with sun and moon shining 
    /// </summary>
    public class DayCycle : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float currentTime;
    
        [FormerlySerializedAs("CurrentTime (s)")]
        [SerializeField] private float dayDuration;
        [SerializeField] private Light sun;
        [SerializeField] private Light moon;
    
        private float _moonLightIntensity;
        private float _sunLightIntensity;
    
        public AnimationCurve sunCurve;
        public AnimationCurve moonCurve;

        /// <summary>
        /// Applies the inspector values to private variables on map starts
        /// </summary>
        private void Start()
        {
            currentTime = .2f;
            _sunLightIntensity = sun.intensity;
            _moonLightIntensity = moon.intensity;
        }

        /// <summary>
        /// Updates current values of sun and moon shining
        /// </summary>
        private void Update()
        {
            currentTime += Time.deltaTime / dayDuration;
            if (currentTime >= 1) currentTime -= 1;

            sun.transform.localRotation = Quaternion.Euler(currentTime * 360f, 180, 0);
            moon.transform.localRotation = Quaternion.Euler(currentTime * 360f + 180f, 180, 0);

            sun.intensity = _sunLightIntensity * sunCurve.Evaluate(currentTime);
            moon.intensity = _moonLightIntensity * moonCurve.Evaluate(currentTime);
        }
    }
}
