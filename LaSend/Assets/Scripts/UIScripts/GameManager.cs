using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    string dataFileName = "LaSData.Xml";
    XmlManager xm = new XmlManager();
    public PlayerData pd;

    public GameObject lPlayer, sPlayer;
    public Vector3 lPlayer_Pos, sPlayer_Pos;
    

    [HideInInspector]
    public bool pause;
    [HideInInspector]
    public bool gameclear;
    [HideInInspector]
    public bool gameover;

    public int level;
    public int scenenum;
    private int maxlevel;

    private void Awake()
    {
    }
    // Use this for initialization
    void Start () {
        pause = false;
        gameclear = false;
        gameover = false;
        Load();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitPlayers()
    {
        lPlayer.transform.position = lPlayer_Pos;
    }

    public void LoadGame()
    {
        Load();
        lPlayer.transform.position = pd.lPlayer_Pos;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
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
