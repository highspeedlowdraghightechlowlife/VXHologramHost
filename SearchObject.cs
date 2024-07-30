using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class SearchObject : MonoBehaviour
{
    private static DataService ds;

    void Start()
    {
        ds = new DataService("Objects.db");


        // Debug.Log("Object get: " + rosie);
        // Debug.Log(rosie.getPosition());
    }

    void Update()
    {

    }

    private static string readFile()
    {
        string filePath = Application.dataPath + "/TestFile/sample_data.txt";

        string[] textAssets = File.ReadAllLines(filePath);

        string return_string = textAssets[3];

        // foreach(string line in textAssets)
        // {
        //     return_string = line;
        // }

        return return_string;
    }

    /**
    * this is a test of idea, the dilivered product will not work like this
    * idealy, this function will search based on method by user setting
    */
    public static LabAsset findObjectInGame(string name)
    {
        // Vector3 position = Vector3.zero;
        LabAsset result = new LabAsset();
        try
        {
            result = ds.GetObjectByName(name);
            // position = result.getPosition();
        }
        catch (Exception ex)
        {
            result = null;
            Debug.Log(ex.Message);
        }

        // Debug.Log("read posi: " + position);

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
