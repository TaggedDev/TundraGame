using Player.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class TemperatureIndicatorController : MonoBehaviour
    {

        private Text _textComponent;
        private PlayerBehaviour _behaviour;

        [SerializeField]
        private GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            _textComponent = GetComponent<Text>();
            _behaviour = player.GetComponent<PlayerBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {
            Color resultingColor;
            float tempAmplitudeScale = (_behaviour.CurrentTemperature - _behaviour.PerfectTemperature) / _behaviour.AbsoluteTemperatureAmplitude;
            if (tempAmplitudeScale > 0)
            {
                resultingColor = new Color(1, Mathf.Clamp(1 - tempAmplitudeScale, 0, 1), Mathf.Clamp(1 - tempAmplitudeScale, 0, 1));
            }
            else
            {
                resultingColor = new Color(Mathf.Clamp(1 + tempAmplitudeScale, 0, 1), Mathf.Clamp(1 + tempAmplitudeScale, 0, 1), 1);
            }
            _textComponent.color = resultingColor;
            _textComponent.text = (Math.Round(_behaviour.CurrentTemperature, 1) + " C").Replace(',', '.');
            print(tempAmplitudeScale);
        }
    }
}

