using UnityEngine;
namespace Bang.Lib.ObjectPooling
{
    public class ReturnToMyPool : MonoBehaviour
    {
        public MyPool pool;
        public virtual void OnDisable()
        {
            pool.AddToPool(gameObject);
        }
    }

}
