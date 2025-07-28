using UnityEngine;

namespace com.ab.papercrafts
{
    public static class GameObjectExtensions
    {
        public static GameObject WithName(this GameObject source, string name)
        {
            source.name = name;
            return source;
        }
    }
}