using System.ComponentModel;
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


        private readonly NotifyingObject<Color> _buttonEnabledColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonEnabledTextColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledTextColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonEnabledProgressBarColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledProgressBarColorProperty = new NotifyingObject<Color>();

        [DisplayName("button enabled color")]
        public NotifyingObject<Color> ButtonEnabledColorProperty
        {
            get { return _buttonEnabledColorProperty; }
        }

        [DisplayName("button disabled color")]
        public NotifyingObject<Color> ButtonDisabledColorProperty
        {
            get { return _buttonDisabledColorProperty; }
        }

        [DisplayName("button enabled text color")]
        public NotifyingObject<Color> ButtonEnabledTextColorProperty
        {
            get { return _buttonEnabledTextColorProperty; }
        }

        [DisplayName("button disabled color")]
        public NotifyingObject<Color> ButtonDisabledTextColorProperty
        {
            get { return _buttonDisabledTextColorProperty; }
        }

        [DisplayName("button enabled progress bar color")]
        public NotifyingObject<Color> ButtonEnabledProgressBarColorProperty
        {
            get { return _buttonEnabledProgressBarColorProperty; }
        }

        [DisplayName("button disabled progress bar color")]
        public NotifyingObject<Color> ButtonDisabledProgressBarColorProperty
        {
            get { return _buttonDisabledProgressBarColorProperty; }
        }

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
