using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    public TextMeshProUGUI Win_Score;
    public TextMeshProUGUI Defeat_Score;

    private string PathToSaveFile;
    // Start is called before the first frame update

    private void Awake()
    {
        PathToSaveFile = Application.persistentDataPath + "/Stats.txt";

        if (!File.Exists(PathToSaveFile))
        {
            File.Create(PathToSaveFile);
        }
    }

    private void Start()
    {
        if (File.ReadAllText(PathToSaveFile) == "")
        {
            File.WriteAllText(PathToSaveFile, "0 0");
        }
        else
        {
            string[] Stats = File.ReadAllText(PathToSaveFile).Split(' ');
            Win_Score.text = Stats[0];
            Defeat_Score.text = Stats[1];
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
