using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;


public class Play_Logic : MonoBehaviour
{
    public GameObject EndgameCanvas;
    public GameObject InfoCanvas;
    public TextMeshProUGUI EndGameText;
    public GameObject Enemy;
    public int NumberOfEnemies = 7;

    private GameObject Player;
    private string PathToSaveFile;
    string[] Stats;

    private void Awake()
    {
        for (int i = 0; i < NumberOfEnemies; i++)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(-30,30), 3.53f, Random.Range(-36, 36));
            Instantiate(Enemy, SpawnPos, Quaternion.identity);
        }
        PathToSaveFile = Application.persistentDataPath + "/Stats.txt";
        Stats = File.ReadAllText(PathToSaveFile).Split(' ');
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Count() <= 0)
        {
            EnableCorrectCanvas();
            ShowVictoryScreen();
        }
        if(Player.GetComponent<Player_Controller>().isActiveAndEnabled && Player.GetComponent<Player_Controller>().health <= 0)
        {
            EnableCorrectCanvas();
            ShowDefeatScreen();
        }
    }

    void EnableCorrectCanvas()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        InfoCanvas.SetActive(false);
        EndgameCanvas.SetActive(true);
    }

    void ShowDefeatScreen()
    {
        EndGameText.text = "You lost";
        Player.GetComponent<Player_Controller>().enabled = false;
        Player.GetComponentInChildren<Gun_Shoot>().enabled = false;
        GameObject[] EnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject obj in EnemiesLeft)
        {
            obj.GetComponent<EnemyUnit>().enabled = false;
        }
        Stats[1] = (int.Parse(Stats[1]) + 1).ToString();
        WriteSaveFile();
    }

    void ShowVictoryScreen()
    {
        EndGameText.text = "You won";
        Player.GetComponent<Player_Controller>().enabled = false;
        Player.GetComponentInChildren<Gun_Shoot>().enabled = false;
        Stats[0] = (int.Parse(Stats[0]) + 1).ToString();
        WriteSaveFile();
    }

    public void ReturnToMainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    void WriteSaveFile()
    {
        File.WriteAllText(PathToSaveFile, Stats[0] + " " + Stats[1]);
        Player.GetComponent<Play_Logic>().enabled = false;
    }
}
