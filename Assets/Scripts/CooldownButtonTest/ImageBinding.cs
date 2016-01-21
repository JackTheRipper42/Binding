using System.Configuration.Assemblies;
using Assets.Scripts.Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CooldownButtonTest
{
    [RequireComponent(typeof(Image))]
    public class ImageBinding : MonoBehaviour
    {
        public readonly DependencyProperty<Color> ColorProperty;
        public readonly DependencyProperty<Vector2> AnchorMinProperty;
        public readonly DependencyProperty<Vector2> AnchorMaxProperty; 

        private Image _image;

        public ImageBinding()
        {
            ColorProperty = new DependencyProperty<Color>(
                new BindingFactory(),
                Color.white,
                ColorPropertyChangedCallback,
                null);
            AnchorMinProperty = new DependencyProperty<Vector2>(
                new BindingFactory(),
                new Vector2(0, 0),
                AnchorMinPropertyChangedCallback,
                null);
            AnchorMaxProperty = new DependencyProperty<Vector2>(
                new BindingFactory(),
                new Vector2(1f, 1f),
                AnchorMaxPropertyChangedCallback,
                null);
        }

        protected virtual void Start()
        {
            _image = GetComponent<Image>();

            if (ColorProperty.Bound)
            {
                SetColor(ColorProperty.GetValue());
            }
            if (AnchorMinProperty.Bound)
            {
                SetAnchorMin(AnchorMinProperty.GetValue());
            }
            if (AnchorMaxProperty.Bound)
            {
                SetAnchorMax(AnchorMaxProperty.GetValue());
            }
        }

        protected virtual void OnDestroy()
        {
            _image = null;
        }

        private void SetColor(Color color)
        {
            _image.color = color;
        }

        private void SetAnchorMin(Vector2 anchor)
        {
            _image.rectTransform.anchorMin = anchor;
        }

        private void SetAnchorMax(Vector2 anchor)
        {
            _image.rectTransform.anchorMax = anchor;
        }

        private void ColorPropertyChangedCallback(Color oldValue, Color newValue)
        {
            if (_image == null)
            {
                return;
            }

            SetColor(newValue);
        }

        private void AnchorMaxPropertyChangedCallback(Vector2 oldValue, Vector2 newValue)
        {
            if (_image == null)
            {
                return;
            }

            SetAnchorMax(newValue);
        }

        private void AnchorMinPropertyChangedCallback(Vector2 oldValue, Vector2 newValue)
        {
            if (_image == null)
            {
                return;
            }

            SetAnchorMin(newValue);
        }
    }
}
