using System;
using Assets.Scripts.Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CooldownButtonTest
{
    [RequireComponent(typeof(Button))]
    public class ButtonBinding : MonoBehaviour
    {
        public readonly DependencyProperty<ICommand> CommandProperty;

        private Button _button;

        public ButtonBinding()
        {
            CommandProperty = new DependencyProperty<ICommand>(
                new BindingFactory(),
                null,
                CommandPropertyChangedCallback,
                null);
        }

        protected virtual void Start()
        {
            _button = GetComponent<Button>();

            if (CommandProperty.Bound)
            {
                var command = CommandProperty.GetValue();
                if (command != null)
                {
                    SetCommand(command);
                }
                else
                {
                    _button.enabled = false;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            var command = CommandProperty.GetValue();
            if (command != null)
            {
                ClearCommand(command);
            }

            _button = null;
        }

        private void SetCommand(ICommand command)
        {
            command.CanExecuteChanged += CommandOnCanExecuteChanged;
            _button.onClick.AddListener(command.Execute);
            _button.enabled = command.CanExecute();
        }

        private void ClearCommand(ICommand command)
        {
            command.CanExecuteChanged -= CommandOnCanExecuteChanged;
            _button.onClick.RemoveListener(command.Execute);
            _button.enabled = false;
        }

        private void CommandPropertyChangedCallback(
            ICommand oldValue,
            ICommand newValue)
        {
            if (_button == null)
            {
                return;
            }

            if (oldValue != null)
            {
                ClearCommand(oldValue);
            }

            if (newValue != null)
            {
                SetCommand(newValue);
            }
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            if (_button == null)
            {
                return;
            }

            var command = CommandProperty.GetValue();
            _button.enabled = command.CanExecute();
        }
    }
}