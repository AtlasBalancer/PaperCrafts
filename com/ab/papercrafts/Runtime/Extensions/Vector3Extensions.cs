using UnityEngine;

namespace com.ab.papercrafts
{
    public static class Vector3Extensions
    {
        public static Vector3 DefaultZ(this Vector3 source, float z = default) =>
            new(source.x, source.y, z);

        public static Vector3 OffsetY(this Vector3 source, float offsetY) =>
            new(source.x, source.y + offsetY, source.z);
    }
}