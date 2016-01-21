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

        public void OnBeforeSerialize()
        {
            _buttonEnabledColor = ButtonEnabledColorProperty.GetValue();
            _buttonDisabledColor = ButtonDisabledColorProperty.GetValue();
            _buttonEnabledTextColor = ButtonEnabledTextColorProperty.GetValue();
            _buttonDisabledTextColor = ButtonDisabledTextColorProperty.GetValue();
            _buttonEnabledProgressBarColor = ButtonEnabledProgressBarColorProperty.GetValue();
            _buttonDisabledProgressBarColor = ButtonDisabledProgressBarColorProperty.GetValue();
        }

        public void OnAfterDeserialize()
        {
            ButtonEnabledColorProperty.SetValue(_buttonEnabledColor);
            ButtonDisabledColorProperty.SetValue(_buttonDisabledColor);
            ButtonEnabledTextColorProperty.SetValue(_buttonEnabledTextColor);
            ButtonDisabledTextColorProperty.SetValue(_buttonDisabledTextColor);
            ButtonEnabledProgressBarColorProperty.SetValue(_buttonEnabledProgressBarColor);
            ButtonDisabledProgressBarColorProperty.SetValue(_buttonDisabledProgressBarColor);
        }
    }
}
