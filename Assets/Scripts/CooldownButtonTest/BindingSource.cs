using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    public class BindingSource : MonoBehaviour
    {
        public int Ticks = 0;
        public string Text;

        private NotifyingObject<int> _progress;
        private NotifyingObject<ICommand> _command;

        private int Progress
        {
            get { return _progress.GetValue(); }
            set { _progress.SetValue(value); }
        }

        protected virtual void Start()
        {
            _progress = new NotifyingObject<int>(0);
            _command = new NotifyingObject<ICommand>(new DelegateCommand(() =>
            {
                Ticks++;
                Progress = 0;
            }));

            var resourceManager = GetComponentInParent<ResourceManager>();

            var cooldownButton = GetComponentInChildren<CooldownButton>();
            cooldownButton.EnabledColorProperty.Bind(
                BindingType.OneWay,
                resourceManager.ButtonEnabledColorProperty);
            cooldownButton.DisabledColorProperty.Bind(
                BindingType.OneWay,
                resourceManager.ButtonDisabledColorProperty);
            cooldownButton.EnabledTextColorProperty.Bind(
                BindingType.OneWay,
                resourceManager.ButtonEnabledTextColorProperty);
            cooldownButton.DisabledTextColorProperty.Bind(
                BindingType.OneWay,
                resourceManager.ButtonDisabledTextColorProperty);
            cooldownButton.EnabledProgressBarColorProperty.Bind(
                BindingType.OneWay,
                resourceManager.ButtonEnabledProgressBarColorProperty);
            cooldownButton.DisabledProgessBarColorProperty.Bind(
                BindingType.OneWay,
                resourceManager.ButtonDisabledProgressBarColorProperty);
            cooldownButton.TextProperty.SetValue(Text);
            cooldownButton.ProgressProperty.Bind(BindingType.TwoWay, _progress);
            cooldownButton.CommandProperty.Bind(BindingType.OneWay, _command);
        }

        protected virtual void Update()
        {
            Progress++;
        }
    }
}
