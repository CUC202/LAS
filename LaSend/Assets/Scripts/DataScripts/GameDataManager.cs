using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour {
    string dataFileName = "LaSData.Xml";
    XmlManager xm = new XmlManager();
    public PlayerData pd;
    public GameObject gameManager;
    // Use this for initialization
    void Start () {
        //pd = new PlayerData();
        // Save();
        // Debug.Log("1");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Save()
    {
        string dataFilePath = GetDataPath() + "/" + dataFileName;
        string serializeDataString = xm.SerializeObject(pd, typeof(PlayerData));
        xm.CreatXML(dataFilePath, serializeDataString);
    }

    public string GetDataPath()
    {
        return Application.dataPath;
    }

    public void Load()
    {
        string dataFilePath = GetDataPath() + "/" + dataFileName;
        if (xm.HasFile(dataFilePath))
        {
            string dataString = xm.LoadXML(dataFilePath);
            PlayerData pdFromXML = xm.DeserializeObject(dataString, typeof(PlayerData)) as PlayerData;
            if (pdFromXML != null)
            {
                pd = pdFromXML;
            }
            else
                Debug.Log("xml is null!");
        }
    }


    public void PrintData()
    {
        Debug.Log("pd_pos" + pd.lPlayer_Pos);
    }


}
