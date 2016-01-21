using Assets.Scripts.Binding;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    public class CooldownButton : MonoBehaviour
    {
        public const int ProgressMinValue = 0;
        public const int ProgressMaxValue = 100;

        [Range(0, 100)] public int ProgressBarHeightPercentage = 80;

        public readonly DependencyProperty<ICommand> CommandProperty;
        public readonly DependencyProperty<string> TextProperty;
        public readonly DependencyProperty<Color> EnabledColorProperty;
        public readonly DependencyProperty<Color> DisabledColorProperty;
        public readonly DependencyProperty<Color> EnabledTextColorProperty;
        public readonly DependencyProperty<Color> DisabledTextColorProperty;
        public readonly DependencyProperty<Color> EnabledProgressBarColorProperty;
        public readonly DependencyProperty<Color> DisabledProgessBarColorProperty;
        public readonly DependencyProperty<int> ProgressProperty;

        private readonly CooldownButtonViewModel _viewModel;

        public CooldownButton()
        {
            _viewModel = new CooldownButtonViewModel();

            TextProperty = new DependencyProperty<string>(
                new BindingFactory(),
                string.Empty,
                (oldValue, newValue) => _viewModel.SetText(newValue),
                null);

            EnabledColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.white,
                (oldValue, newValue) => _viewModel.SetEnabledColor(newValue),
                null);

            DisabledColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.gray,
                (oldValue, newValue) => _viewModel.SetDisabledColor(newValue),
                null);

            EnabledTextColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.black,
                (oldValue, newValue) => _viewModel.SetEnabledTextColor(newValue),
                null);

            DisabledTextColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.gray + Color.black,
                (oldValue, newValue) => _viewModel.SetDisabledTextColor(newValue),
                null);

            EnabledProgressBarColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.red,
                (oldValue, newValue) => _viewModel.SetEnabledProgressBarColor(newValue),
                null);

            DisabledProgessBarColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.red,
                (oldValue, newValue) => _viewModel.SetDisabledProgessBarColor(newValue),
                null);

            ProgressProperty = new DependencyProperty<int>(
                new BindingFactory(),
                0,
                (oldValue, newValue) => _viewModel.SetProgress(newValue),
                value => Math.Max(ProgressMinValue, Math.Min(ProgressMaxValue, value)));

            CommandProperty = new DependencyProperty<ICommand>(
                new BindingFactory(),
                null,
                (oldvalue, newValue) => _viewModel.SetCommand(newValue),
                null);
        }

        protected virtual void Start()
        {
            var buttonBinding = GetComponentInChildren<ButtonBinding>();

            var imageBindings = buttonBinding.GetComponentsInChildren<ImageBinding>();

            var buttonImageBinding = imageBindings.Single(binding => ReferenceEquals(
                buttonBinding.gameObject,
                binding.gameObject));
            var panelBinding = imageBindings.Single(binding => ReferenceEquals(
                buttonImageBinding.gameObject,
                binding.gameObject.transform.parent.gameObject));

            var textBinding = buttonBinding.gameObject.GetComponentInChildren<TextBinding>();

            _viewModel.SetEnabledColor(EnabledColorProperty.GetValue());
            _viewModel.SetDisabledColor(DisabledColorProperty.GetValue());
            _viewModel.SetEnabledTextColor(EnabledTextColorProperty.GetValue());
            _viewModel.SetDisabledTextColor(DisabledTextColorProperty.GetValue());
            _viewModel.SetEnabledProgressBarColor(EnabledProgressBarColorProperty.GetValue());
            _viewModel.SetDisabledProgessBarColor(DisabledProgessBarColorProperty.GetValue());
            _viewModel.SetProgressBarHeightPercentage(ProgressBarHeightPercentage);

            buttonBinding.CommandProperty.Bind(BindingType.OneWay, _viewModel.CommandProperty);
            buttonImageBinding.ColorProperty.Bind(BindingType.OneWay, _viewModel.ButtonColorProperty);
            panelBinding.ColorProperty.Bind(BindingType.OneWay, _viewModel.ProgressBarColorPoperty);
            panelBinding.AnchorMinProperty.Bind(BindingType.OneWay, _viewModel.ProgressBarAnchorMinProperty);
            panelBinding.AnchorMaxProperty.Bind(BindingType.OneWay, _viewModel.ProgressBarAnchorMaxProperty);
            textBinding.TextProperty.Bind(BindingType.OneWay, _viewModel.ButtonTextProperty);
            textBinding.ColorProperty.Bind(BindingType.OneWay, _viewModel.ButtonTextColorProperty);
        }
    }
}
