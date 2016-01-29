using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Serialization.Internal
{
    public interface IFormatSelector
    {
        SerializationFormat GetFormat([NotNull] Type type, [CanBeNull] object data);
    }
}