using UnityEngine;

namespace com.ab.papercrafts
{
    public static class Vector3IntExtensions
    {
        public static Vector3Int IncreaseZ(this Vector3Int source, int offset) =>
            new(source.x, source.y, source.z + offset);
        
        public static Vector3Int DefaultZ(this Vector3Int source, int Zindex = 0) =>
            new(source.x, source.y, Zindex);
        public  static Vector2Int ToVector2Int(this Vector3Int source) => 
            new(source.x, source.y);
    }
}