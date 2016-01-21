using Assets.Scripts.Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CooldownButtonTest
{
    [RequireComponent(typeof(Text))]
    public class TextBinding : MonoBehaviour
    {
        public readonly DependencyProperty<string> TextProperty;
        public readonly DependencyProperty<Color> ColorProperty;

        private Text _text;

        public TextBinding()
        {
            TextProperty = new DependencyProperty<string>(
                new BindingFactory(),
                string.Empty,
                TextPropertyChangedCallback,
                null);
            ColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.black,
                ColorPropertyChangedCallback,
                null);
        }

        protected virtual void Start()
        {
            _text = GetComponent<Text>();

            if (TextProperty.Bound)
            {
                SetText(TextProperty.GetValue());
            }
        }

        protected virtual void OnDestroy()
        {
            _text = null;
        }

        private void SetText(string text)
        {
            _text.text = text;
        }

        private void SetColor(Color color)
        {
            _text.color = color;
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
            if (_text == null)
            {
                return;
            }

            SetColor(newValue);
        }
    }
}
