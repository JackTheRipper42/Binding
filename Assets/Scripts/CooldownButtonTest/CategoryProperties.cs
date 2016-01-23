using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.CooldownButtonTest
{
    public class CategoryProperties
    {
        private readonly string _category;
        private readonly IEnumerable<NotifyingObjectProperty> _properties;

        public CategoryProperties([NotNull] string category, [NotNull] IEnumerable<NotifyingObjectProperty> properties)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            _category = category;
            _properties = properties;
        }

        public string Category
        {
            get { return _category; }
        }

        public IEnumerable<NotifyingObjectProperty> Properties
        {
            get { return _properties; }
        }
    }
}
