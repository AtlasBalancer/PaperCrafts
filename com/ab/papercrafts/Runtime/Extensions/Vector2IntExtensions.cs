using UnityEngine;

namespace com.ab.papercrafts
{
    public static class Vector2IntExtensions
    {
        public static Vector3Int ToVector3Int(this Vector2Int source, int ZIndex = 0) => 
            new(source.x, source.y, ZIndex);
    }
}