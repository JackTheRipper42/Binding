using JetBrains.Annotations;

namespace Assets.Scripts.Serialization.Internal
{
    public interface IBinarySerializer
    {
        object Deserialize([NotNull] byte[] binaryData);
        byte[] Serialize(object data);
    }
}