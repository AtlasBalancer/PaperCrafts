using UnityEngine;

namespace com.ab.papercrafts
{
    public static class BehaviourExtensions
    {
       public static void SetPosition(this Behaviour source, Vector3 position) => 
            source.transform.position = position;
        
        public static void Active(this Behaviour source, bool active) => 
            source.gameObject.SetActive(active);
    }
}