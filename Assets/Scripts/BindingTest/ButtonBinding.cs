using Assets.Scripts.Binding;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BindingTest
{
    public class ButtonBinding : MonoBehaviour
    {
        public readonly DependencyProperty<ICommand> CommandProperty;
        public readonly DependencyProperty<string> TextProperty;
        public readonly DependencyProperty<Color> ColorProperty;
        public readonly DependencyProperty<Color> TextColorProperty; 

        private Button _button;
        private Text _text;
        private Image _image;

        public ButtonBinding()
        {
            CommandProperty = new DependencyProperty<ICommand>(
                new BindingFactory(),
                null,
                CommandPropertyChangedCallback,
                null);
            TextProperty = new DependencyProperty<string>(
                new BindingFactory(),
                string.Empty,
                TextPropertyChangedCallback,
                null);
            ColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.white,
                ColorPropertyChangedCallback,
                null);
            TextColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.black,
                TextColorPropertyChangedCallback,
                null);
        }

        protected virtual void Start()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<Text>();

            if (TextProperty.Bound)
            {
                SetText(TextProperty.GetValue());
            }
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
            _text = null;
            _image = null;
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

        private void SetText(string text)
        {
            _text.text = text;
        }

        private void SetColor(Color color)
        {
            _image.color = color;
        }

        private void SetTextColor(Color color)
        {
            _text.color = color;
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

        private void TextPropertyChangedCallback(string oldValue, string newValue)
        {
            if (_text == null)
            {
                return;
            }

            SetText(newValue);
        }

        private void ColorPropertyChangedCallback(Color oldValue, Color newValue)
        {
            if (_image == null)
            {
                return;
            }

            SetColor(newValue);
        }

        private void TextColorPropertyChangedCallback(Color oldValue, Color newValue)
        {
            if (_text == null)
            {
                return;
            }

            SetTextColor(newValue);
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
