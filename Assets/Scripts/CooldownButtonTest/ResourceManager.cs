using Assets.Scripts.Binding;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    public class ResourceManager : ResourceManagerBase
    {
        private readonly NotifyingObject<Color> _buttonEnabledColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonEnabledTextColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledTextColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonEnabledProgressBarColorProperty = new NotifyingObject<Color>();
        private readonly NotifyingObject<Color> _buttonDisabledProgressBarColorProperty = new NotifyingObject<Color>();       

        [DisplayName("button enabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonEnabledColorProperty
        {
            get { return _buttonEnabledColorProperty; }
        }

        [DisplayName("button disabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonDisabledColorProperty
        {
            get { return _buttonDisabledColorProperty; }
        }

        [DisplayName("text enabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonEnabledTextColorProperty
        {
            get { return _buttonEnabledTextColorProperty; }
        }

        [DisplayName("text disabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonDisabledTextColorProperty
        {
            get { return _buttonDisabledTextColorProperty; }
        }

        [DisplayName("progress bar enabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonEnabledProgressBarColorProperty
        {
            get { return _buttonEnabledProgressBarColorProperty; }
        }

        [DisplayName("progress bar disabled")]
        [Category("button colors")]
        public NotifyingObject<Color> ButtonDisabledProgressBarColorProperty
        {
            get { return _buttonDisabledProgressBarColorProperty; }
        }
    }
}
