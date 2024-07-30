using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SQLite4Unity3d;

public class LabAsset  {

	public string name { get; set; }
	public float x_posi { get; set; }
	public float y_posi { get; set; }
	public float z_posi { get; set; }
	public string labels { get; set; }
	public string description { get; set; }
	public string location_info { get; set; }


    private List<string> label_list;
    private bool list_broken = false;

    public Vector3 getPosition()
    {
        Vector3 position = new Vector3(x_posi, y_posi, z_posi);
        return position;
    }

    private void breakLabels2List()
    {
        string[] splitStrings = labels.Split(',');
        List<string> label_list = new List<string>(splitStrings);
        list_broken = true;
    }

    public bool isInLabelList(string label_2_search)
    {
        bool exist = false;
        if (!list_broken)
        {
            breakLabels2List();
        }
        
        if (label_list.Contains(label_2_search))
        {
            exist = true;
        }

        return exist;
    }


	public override string ToString()
	{
		return string.Format ("[Object: name={0}, x={1}, y={2}, z={3}, labels={4}, description={5}, location_info={6}]", 
                                name, x_posi, y_posi, z_posi, labels, description, location_info);
	}
}