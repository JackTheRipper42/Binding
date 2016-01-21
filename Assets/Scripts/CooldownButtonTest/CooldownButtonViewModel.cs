using Assets.Scripts.Binding;
using System;
using UnityEngine;

namespace Assets.Scripts.CooldownButtonTest
{
    public class CooldownButtonViewModel
    {
        public readonly NotifyingObject<Vector2> ProgressBarAnchorMinProperty;
        public readonly NotifyingObject<Vector2> ProgressBarAnchorMaxProperty;
        public readonly NotifyingObject<Color> ProgressBarColorPoperty;
        public readonly NotifyingObject<Color> ButtonColorProperty;
        public readonly NotifyingObject<Color> ButtonTextColorProperty;
        public readonly NotifyingObject<string> ButtonTextProperty;
        public readonly NotifyingObject<ICommand> CommandProperty;

        private readonly DelegateCommand _cooldownButtonCommand;

        private Color _enabledColor;
        private Color _disabledColor;
        private Color _enabledTextColor;
        private Color _disabledTextColor;
        private Color _enabledProgressBarColor;
        private Color _disabledProgessBarColor;
        private int _progress;
        private ICommand _command;
        private bool _enabled;
        private int _progressBarHeightPercentage;

        public CooldownButtonViewModel()
        {
            ProgressBarAnchorMinProperty = new NotifyingObject<Vector2>();
            ProgressBarAnchorMaxProperty = new NotifyingObject<Vector2>();
            ProgressBarColorPoperty = new NotifyingObject<Color>();
            ButtonColorProperty = new NotifyingObject<Color>();
            ButtonTextProperty = new NotifyingObject<string>();
            ButtonTextColorProperty = new NotifyingObject<Color>();
            CommandProperty = new NotifyingObject<ICommand>();

            _cooldownButtonCommand = new DelegateCommand(CommandExecute, CommandCanExecute);
            CommandProperty.SetValue(_cooldownButtonCommand);
        }

        public void SetEnabledColor(Color color)
        {
            _enabledColor = color;
            UpdateColor();
        }

        public void SetDisabledColor(Color color)
        {
            _disabledColor = color;
            UpdateColor();
        }

        public void SetEnabledTextColor(Color color)
        {
            _enabledTextColor = color;
            UpdateTextColor();
        }

        public void SetDisabledTextColor(Color color)
        {
            _disabledTextColor = color;
            UpdateTextColor();
        }

        public void SetEnabledProgressBarColor(Color color)
        {
            _enabledProgressBarColor = color;
            UpdateProgressBarColor();
        }

        public void SetDisabledProgessBarColor(Color color)
        {
            _disabledProgessBarColor = color;
            UpdateProgressBarColor();
        }

        public void SetProgress(int value)
        {
            _progress = value;
            RaiseCanExecuteChanged();
            UpdateEnabled();
            UpdateProgressBarDimensions();
        }

        public void SetText(string text)
        {
            ButtonTextProperty.SetValue(text);
        }

        public void SetCommand(ICommand command)
        {
            if (_command != null)
            {
                _command.CanExecuteChanged -= CommandOnCanExecuteChanged;
            }

            _command = command;

            _command.CanExecuteChanged += CommandOnCanExecuteChanged;
            RaiseCanExecuteChanged();
        }

        public void SetProgressBarHeightPercentage(int percentage)
        {
            _progressBarHeightPercentage = percentage;
            UpdateProgressBarDimensions();
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            RaiseCanExecuteChanged();
        }

        private void CommandExecute()
        {
            if (_command != null)
            {
                _command.Execute();
            }
        }

        private bool CommandCanExecute()
        {
            UpdateEnabled();
            return _enabled;
        }

        private void RaiseCanExecuteChanged()
        {
            _cooldownButtonCommand.RaiseCanExecuteChanged();
        }

        private void UpdateEnabled()
        {
            if (_command != null)
            {
                _enabled = _command.CanExecute() && _progress == CooldownButton.ProgressMaxValue;
            }
            else
            {
                _enabled = _progress == CooldownButton.ProgressMaxValue;
            }
            UpdateColor();
            UpdateTextColor();
            UpdateProgressBarColor();
        }

        private void UpdateColor()
        {
            var color = _enabled ? _enabledColor : _disabledColor;
            ButtonColorProperty.SetValue(color);
        }

        private void UpdateTextColor()
        {
            var color = _enabled ? _enabledTextColor : _disabledTextColor;
            ButtonTextColorProperty.SetValue(color);
        }

        private void UpdateProgressBarColor()
        {
            var color = _enabled ? _enabledProgressBarColor : _disabledProgessBarColor;
            ProgressBarColorPoperty.SetValue(color);
        }

        private void UpdateProgressBarDimensions()
        {
            // ToDo: clean up the mess

            ProgressBarAnchorMinProperty.SetValue(new Vector2(0.5f, (1f - _progressBarHeightPercentage/100f)/2f));
            ProgressBarAnchorMaxProperty.SetValue(new Vector2(
                0.5f,
                _progressBarHeightPercentage*_progress/10000f + (1 - _progressBarHeightPercentage/100f)/2f));
        }
    }
}
