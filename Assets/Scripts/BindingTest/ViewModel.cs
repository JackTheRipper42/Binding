using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts.BindingTest
{
    public class ViewModel
    {
        public readonly NotifyingObject<Vector3> PositionProperty;
        public readonly NotifyingObject<string> UpButtonTextProperty;
        public readonly NotifyingObject<string> DownButtonTextProperty;
        public readonly NotifyingObject<float> UpThresholdProperty;
        public readonly NotifyingObject<float> DownThresholdProperty;
        public readonly NotifyingObject<ICommand> UpButtonCommandProperty;
        public readonly NotifyingObject<ICommand> DownButtonCommandProperty;
        public readonly NotifyingObject<bool> UpAvailableProperty;
        public readonly NotifyingObject<bool> DownAvailableProperty; 

        private readonly Vector3 _upDownVector = new Vector3(0f, 0.5f, 0f);

        public ViewModel()
        {
            PositionProperty = new NotifyingObject<Vector3>();
            UpButtonTextProperty = new NotifyingObject<string>();
            DownButtonTextProperty = new NotifyingObject<string>();
            UpThresholdProperty = new NotifyingObject<float>();
            DownThresholdProperty = new NotifyingObject<float>();
            UpButtonCommandProperty = new NotifyingObject<ICommand>();
            DownButtonCommandProperty = new NotifyingObject<ICommand>();
            DownButtonTextProperty = new NotifyingObject<string>();
            UpAvailableProperty = new NotifyingObject<bool>();
            DownAvailableProperty = new NotifyingObject<bool>();

            PositionProperty.PropertyChanged += (o, e) =>
            {
                SetUpAvailable();
                SetDownAvailable();
            };
            SetUpAvailable();
            SetDownAvailable();
            UpAvailableProperty.PropertyChanged += (o, e) => SetUpAvailable();
            DownAvailableProperty.PropertyChanged += (o, e) => SetDownAvailable();

            var upButtonCommand = new DelegateCommand(
                () => Position += _upDownVector,
                () => UpAvailable);
            PositionProperty.PropertyChanged += (o, e) => upButtonCommand.RaiseCanExecuteChanged();
            UpThresholdProperty.PropertyChanged += (o, e) => upButtonCommand.RaiseCanExecuteChanged();
            UpButtonCommandProperty.SetValue(upButtonCommand);

            var downButtonCommand = new DelegateCommand(
                () => Position -= _upDownVector,
                () => DownAvailable);
            PositionProperty.PropertyChanged += (o, e) => downButtonCommand.RaiseCanExecuteChanged();
            DownThresholdProperty.PropertyChanged += (o, e) => downButtonCommand.RaiseCanExecuteChanged();
            DownButtonCommandProperty.SetValue(downButtonCommand);
        }

        public Vector3 Position
        {
            get { return PositionProperty.GetValue(); }
            set { PositionProperty.SetValue(value); }
        }

        public string UpButtonText
        {
            get { return UpButtonTextProperty.GetValue(); }
            set { UpButtonTextProperty.SetValue(value); }
        }

        public string DownButtonText
        {
            get { return DownButtonTextProperty.GetValue(); }
            set { DownButtonTextProperty.SetValue(value); }
        }

        public float UpThreshold
        {
            get { return UpThresholdProperty.GetValue(); }
            set { UpThresholdProperty.SetValue(value); }
        }

        public float DownThreshold
        {
            get { return DownThresholdProperty.GetValue(); }
            set { DownThresholdProperty.SetValue(value); }
        }

        public bool UpAvailable
        {
            get { return UpAvailableProperty.GetValue(); }
            set { UpAvailableProperty.SetValue(value); }
        }

        public bool DownAvailable
        {
            get { return DownAvailableProperty.GetValue(); }
            set { DownAvailableProperty.SetValue(value); }
        }

        private void SetUpAvailable()
        {
            UpAvailable = Position.y < UpThreshold;
        }

        private void SetDownAvailable()
        {
            DownAvailable = Position.y > DownThreshold;
        }
    }
}
