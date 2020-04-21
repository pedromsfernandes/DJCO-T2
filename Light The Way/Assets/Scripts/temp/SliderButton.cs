using Lighting;
using TMPro;
using UnityEngine;

namespace temp
{
    public class SliderButton : MonoBehaviour
    {
        private float _value;
        private TextMeshProUGUI _textMeshPro;
        private SunController _sunController;
        
        // Start is called before the first frame update
        void Start()
        {
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            _sunController = GameObject.FindWithTag("Sun").GetComponent<SunController>();
            
            UpdateText(9);
            UpdateSun();
        }

        public void UpdateText(float value)
        {
            _value = value;
            _textMeshPro.SetText($"Update Sun: {value}");
        }

        public void UpdateSun()
        {
            _sunController.SetSunHour(_value);
        }
    }
}
