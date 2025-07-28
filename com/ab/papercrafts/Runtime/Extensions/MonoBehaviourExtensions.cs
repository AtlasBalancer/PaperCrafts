using UnityEngine;

namespace com.ab.papercrafts
{
    public static class MonoBehaviourExtensions
    {
        public static MonoBehaviour WithName(this MonoBehaviour source, string name)
        {
            source.name = name;
            return source;
        }       
    }
}