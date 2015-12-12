using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts.BindingTest
{
    public class BindingTarget : MonoBehaviour
    {
        public readonly DependencyProperty<Vector3> PositionProperty;

        public BindingTarget()
        {
            PositionProperty = new DependencyProperty<Vector3>(
                new BindingFactory(),
                new Vector3(), 
                PropertyChangedCallback,
                null);
        }

        private void PropertyChangedCallback(Vector3 oldValue, Vector3 newValue)
        {
            if (gameObject.activeInHierarchy)
            {
                transform.position = newValue;
            }
        }

        // Use this for initialization
        private void Start()
        {
            PositionProperty.SetValue(transform.position);
        }

        // Update is called once per frame
        private void Update()
        {
            PositionProperty.SetValue(transform.position);
        }

        public void OnDestroy()
        {
            PositionProperty.ClearBinding();
        }
    }
}
