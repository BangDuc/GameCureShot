using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{
    [SerializeField]Dictionary<string, ItemData> itemLibrary;

    List<ItemData> DataShow;
    void Start()
    {
        itemLibrary = new Dictionary<string,ItemData>();
        DataShow = new List<ItemData>();

    }

    public void GetDataItemLibrary()
    {

    }

    void Update()
    {
        
    }
}

