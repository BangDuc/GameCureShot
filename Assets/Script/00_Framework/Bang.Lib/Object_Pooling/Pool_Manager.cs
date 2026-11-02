using Bang.Lib.Singleton;
using System.Collections.Generic;
using UnityEngine;
namespace Bang.Lib.ObjectPooling
{
    public class Pool_Manager : Singleton<Pool_Manager>
    {
 
        private Dictionary<GameObject, MyPool> dicPools = new Dictionary<GameObject, MyPool>();
        public GameObject GetFromPool(GameObject obj)
        {
            if (dicPools.ContainsKey(obj) == false)
            {
                dicPools.Add(obj, new MyPool(obj));
            }
            return dicPools[obj].Get();
        }
    }
}

