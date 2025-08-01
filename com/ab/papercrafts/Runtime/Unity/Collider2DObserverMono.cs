using UnityEngine;

namespace com.ab.papercrafts.PaperCrafts
{
    [RequireComponent(typeof(Collider2D))]
    public class Collider2DObserverMono : MonoBehaviour
    {
        ColliderHandler _handler;
        
        void OnTriggerEnter(Collider other) => 
            _handler.Enter(other);

        void OnTriggerExit(Collider other) => 
            _handler.Exit(other);

        void OnTriggerStay(Collider other) => 
            _handler.Stay(other);
    }

    public interface ColliderHandler
    {
        public void Enter(Collider other);
        public void Exit(Collider other);

        public void Stay(Collider other);
    }
}