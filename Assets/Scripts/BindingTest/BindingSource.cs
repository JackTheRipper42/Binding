using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts.BindingTest
{
    public class BindingSource : MonoBehaviour
    {
        public BindingTarget BindingTarget;

        private readonly NotifyingObject<Vector3> _position;
        private float _angle;

        public BindingSource()
        {
            _position = new NotifyingObject<Vector3>();
        }

        private void Start()
        {
            BindingTarget.PositionProperty.Bind(BindingType.OneWay, _position);
        }

        private void Update()
        {
            _position.Value += new Vector3(1, 0, -1)*Mathf.Sin(_angle) * Time.deltaTime * 5;
            _angle += 0.05f;       
        }
    }
}
