using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts.BindingTest
{
    public class BindingSource : MonoBehaviour
    {
        public BindingTarget BindingTarget;
        public ButtonBinding UpButton;
        public ButtonBinding DownButton;

        public Color EnabledColor;
        public Color DisabledColor;
        
        private readonly ViewModel _viewModel;

        private float _angle;

        public BindingSource()
        {
            _viewModel = new ViewModel
            {
                UpThreshold = 2.5f,
                DownThreshold = -2.5f,
                UpButtonText = "Up",
                DownButtonText = "Down"
            };
        }

        protected virtual void Start()
        {
            var colorConverter = new OneWayDelegateValueConverter<bool, Color>(
                (available, culture) => available
                    ? EnabledColor
                    : DisabledColor);

            BindingTarget.PositionProperty.Bind(BindingType.OneWay, _viewModel.PositionProperty);
            UpButton.CommandProperty.Bind(BindingType.OneWay, _viewModel.UpButtonCommandProperty);
            UpButton.TextProperty.Bind(BindingType.OneWay, _viewModel.UpButtonTextProperty);
            UpButton.TextColorProperty.Bind(_viewModel.UpAvailableProperty, colorConverter);
            DownButton.CommandProperty.Bind(BindingType.OneWay, _viewModel.DownButtonCommandProperty);
            DownButton.TextProperty.Bind(BindingType.OneWay, _viewModel.DownButtonTextProperty);
            DownButton.TextColorProperty.Bind(_viewModel.DownAvailableProperty, colorConverter);
        }

        protected virtual void Update()
        {
            _viewModel.Position += new Vector3(1, 0, -1) * Mathf.Sin(_angle) * Time.deltaTime * 5;
            _angle += 0.05f;
        }
    }
}
