using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// think of this as a controller
// currently only supports local sql database in Assets
public class SearchData : MonoBehaviour
{

    private static DataService ds;
    // Start is called before the first frame update

    void Start()
    {
        ds = new DataService("Objects.db");
        
        
        // Debug.Log("Object get: " + rosie);
        // Debug.Log(rosie.getPosition());
    }

    void Update()
    {
        
    }

    /**
    * this is a test of idea, the dilivered product will not work like this
    * idealy, this function will search based on method by user setting
    */
    public static LabAsset findObjectInGame(string name)
    {
        Vector3 position = Vector3.zero;
        LabAsset result = new LabAsset();

        result = ds.GetObjectByName(name);

        if (result == null)
        {
            result.description = "it seems we do not have the information logged.";
        }

        // // try catch might not be recongnized
        // try
        // {
        //     result = ds.GetObjectByName(name);
        //     position = result.getPosition();
        // }
        // catch (Exception ex)
        // {
        //     Debug.Log(ex.Message);
        // }

        return result;
    }

    private static Vector3 strToVector3(string str_vector3)
    {
        // Debug.Log("converting");
        string[] splitStrings = str_vector3.Split(':');
        string[] position_strs = splitStrings[1].Split(',');
        float vec_x = float.Parse(position_strs[0]);
        float vec_y = float.Parse(position_strs[1]);
        float vec_z = float.Parse(position_strs[2]);

        Vector3 targetDirection = new Vector3(vec_x, vec_y, vec_z);

        return targetDirection;
    }
    
}
