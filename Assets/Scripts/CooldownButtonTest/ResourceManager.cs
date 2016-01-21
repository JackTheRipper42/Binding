using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    public class ResourceManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private Color _buttonEnabledColor;
        [SerializeField] private Color _buttonDisabledColor;
        [SerializeField] private Color _buttonEnabledTextColor;
        [SerializeField] private Color _buttonDisabledTextColor;
        [SerializeField] private Color _buttonEnabledProgressBarColor;
        [SerializeField] private Color _buttonDisabledProgressBarColor;

        public readonly NotifyingObject<Color> ButtonEnabledColorProperty = new NotifyingObject<Color>();
        public readonly NotifyingObject<Color> ButtonDisabledColorProperty = new NotifyingObject<Color>();
        public readonly NotifyingObject<Color> ButtonEnabledTextColorProperty = new NotifyingObject<Color>();
        public readonly NotifyingObject<Color> ButtonDisabledTextColorProperty = new NotifyingObject<Color>();
        public readonly NotifyingObject<Color> ButtonEnabledProgressBarColorProperty = new NotifyingObject<Color>();
        public readonly NotifyingObject<Color> ButtonDisabledProgressBarColorProperty = new NotifyingObject<Color>();

        public Color ButtonEnabledColor
        {
            get { return ButtonEnabledColorProperty.GetValue(); }
            set { ButtonEnabledColorProperty.SetValue(value); }
        }

        public Color ButtonDisabledColor
        {
            get { return ButtonDisabledColorProperty.GetValue(); }
            set { ButtonDisabledColorProperty.SetValue(value); }
        }

        public Color ButtonEnabledTextColor
        {
            get { return ButtonEnabledTextColorProperty.GetValue(); }
            set { ButtonEnabledTextColorProperty.SetValue(value); }
        }

        public Color ButtonDisabledTextColor
        {
            get { return ButtonDisabledTextColorProperty.GetValue(); }
            set { ButtonDisabledTextColorProperty.SetValue(value); }
        }

        public Color ButtonEnabledProgressBarColor
        {
            get { return ButtonEnabledProgressBarColorProperty.GetValue(); }
            set { ButtonEnabledProgressBarColorProperty.SetValue(value); }
        }

        public Color ButtonDisabledProgressBarColor
        {
            get { return ButtonDisabledProgressBarColorProperty.GetValue(); }
            set { ButtonDisabledProgressBarColorProperty.SetValue(value); }
        }

        public void OnBeforeSerialize()
        {
            _buttonEnabledColor = ButtonEnabledColor;
            _buttonDisabledColor = ButtonDisabledColor;
            _buttonEnabledTextColor = ButtonEnabledTextColor;
            _buttonDisabledTextColor = ButtonDisabledTextColor;
            _buttonEnabledProgressBarColor = ButtonEnabledProgressBarColor;
            _buttonDisabledProgressBarColor = ButtonDisabledProgressBarColor;
        }

        public void OnAfterDeserialize()
        {
            ButtonEnabledColor = _buttonEnabledColor;
            ButtonDisabledColor = _buttonDisabledColor;
            ButtonEnabledTextColor = _buttonEnabledTextColor;
            ButtonDisabledTextColor = _buttonDisabledTextColor;
            ButtonEnabledProgressBarColor = _buttonEnabledProgressBarColor;
            ButtonDisabledProgressBarColor = _buttonDisabledProgressBarColor;
        }
    }
}
