using JetBrains.Annotations;
using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public class PropertyBinding<TSource, TTarget> : IDisposable
    {
        private readonly IDependecyProperty<TTarget> _target;
        private readonly INotifingObject<TSource> _source;
        private readonly ValueConverter<TSource, TTarget> _converter; 

        public PropertyBinding(
            BindingType bindingType,
            [NotNull] IDependecyProperty<TTarget> target,
            [NotNull] INotifingObject<TSource> source,
            [NotNull] ValueConverter<TSource,TTarget> converter)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            _target = target;
            _source = source;
            _converter = converter;

            switch (bindingType)
            {
                case BindingType.OneWay:
                    _source.PropertyChanged += SourceOnPropertyChanged;
                    break;
                case BindingType.TwoWay:
                    _source.PropertyChanged += SourceOnPropertyChanged;
                    _target.PropertyChanged += TargetOnPropertyChanged;
                    break;
                case BindingType.OneWayToSource:
                    _target.PropertyChanged += TargetOnPropertyChanged;
                    break;
                default:
                    throw new NotSupportedException(string.Format(
                        "The binding type '{0}' is not supported.",
                        bindingType));
            }
        }

        public CultureInfo Culture { get; set; }

        private void SourceOnPropertyChanged(object sender, PropertyChangedEventArgs<TSource> args)
        {
            if (_converter.CanConvert(args.NewValue, Culture))
            {
                var converted = _converter.Convert(args.NewValue, Culture);
                _target.SetValue(converted);
            }
        }

        private void TargetOnPropertyChanged(object sender, PropertyChangedEventArgs<TTarget> args)
        {
            if (_converter.CanConvertBack(args.NewValue, Culture))
            {
                var converted = _converter.ConvertBack(args.NewValue, Culture);
                _source.SetValue(converted);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            _source.PropertyChanged -= SourceOnPropertyChanged;
            _target.PropertyChanged -= TargetOnPropertyChanged;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PropertyBinding()
        {
            Dispose(false);
        }
    }
}
