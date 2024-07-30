using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// cab be used for more complex data processing
public class ObjectManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static LabAsset GetObject(string request)
    {
        LabAsset result = SearchObject.findObjectInGame(request);

        return result;
    }

}
