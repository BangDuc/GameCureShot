using System.Collections.Generic;
using UnityEngine;
namespace Bang.Lib.ObjectPooling
{
    public class MyPool 
    {
        private Stack<GameObject> mypool = new Stack<GameObject>();
        private GameObject baseObject;
        private GameObject tmp;
        private ReturnToMyPool returnPool;

        public MyPool(GameObject baseObj)
        {

            baseObject = baseObj;
        }
        public GameObject Get()
        {
            if (mypool.Count > 0)
            {
                tmp = mypool.Pop();
                tmp.SetActive(true);
                return tmp;
            }
            tmp = GameObject.Instantiate(baseObject,Pool_Manager.Instance.transform);
            //returnPool = tmp.AddComponent<ReturnToMyPool>();
            returnPool = tmp.GetComponent<ReturnToMyPool>();
            
            if (returnPool == null)
            {
                returnPool = tmp.AddComponent<ReturnToMyPool>();
            }
            returnPool.pool = this;
            return tmp;
        }
        public void AddToPool(GameObject obj)
        {
            mypool.Push(obj);
        }
    }

}
