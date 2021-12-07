using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Enemy Setting")]
    [SerializeField] private int enemyCount;
    [SerializeField] private int enemyHealth = 3;
    [SerializeField] private int enemyCoin=29;
    [SerializeField] private int enemyLHealth = 9;
    [SerializeField] private int enemyLCoin = 68;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyParent;


    [Header("Spawn Setting")]
    [SerializeField] private int gatesCount=9;
    [SerializeField] private float gatesDistance = 15f;
    [SerializeField] private GameObject gatesParent;
    [SerializeField] private GameObject[] gatesObj;

    [Header("UI Manager")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text winBottonCoin;
    [SerializeField] private TMP_Text winBottonCoinAds;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject startPanel;

    [Header("Skybox")]
    [SerializeField] private Material[] skyBox;

    private float valueHelperGates;
    private float valueHelperEnemy;
    private float enemySpawnpoint_1;
    private float enemySpawnpoint_2;
    private int totalCoin;
    private int nowCoin=0;
    private float xCoin=1;
    private int level;
    GateManager gateManager;

    private void Start()
    {
        RenderSettings.skybox = skyBox[Random.Range(0, skyBox.Length)]; //change skybox
        //spawn objects
        SpawnGates(gatesDistance);
        SpawnEnemy(gatesDistance);
        LevelText(); //level up func
        //BinarySerializer.DeleteAllDataFiles();
        //PlayerPrefs.SetInt("level",0);
        //PlayerPrefs.SetInt("GameStartFirstTime", 0);
    }
    void Update() //uý controls
    {
        totalCoin = GameDataManager.GetCoins();
        coinText.text = totalCoin.ToString();
        winBottonCoin.text = nowCoin.ToString();
        winBottonCoinAds.text = ((int)(nowCoin * xCoin)).ToString();
    }

    public void SpawnGates(float gatesDistance)
    {
        valueHelperGates = gatesDistance;
        for (int i =0; i < gatesCount; i++)
        {
            int rand = Random.Range(0, gatesObj.Length);
            GameObject gate = Instantiate(gatesObj[rand]);
            if (rand == 3)
            {
                gateManager = gate.gameObject.GetComponent<GateManager>();
                gateManager.isMoving = true;
            }
            gate.transform.position = gate.transform.position + new Vector3(0, 0, valueHelperGates);
            valueHelperGates = valueHelperGates + gatesDistance;
            gate.transform.parent = gatesParent.transform;
        }
        
    }
    private void SpawnEnemy(float gatesDistance)
    {
        valueHelperEnemy = gatesDistance;
        enemySpawnpoint_1 = gatesDistance + 2f;
        enemySpawnpoint_2 = enemySpawnpoint_1 + gatesDistance - 4f;

       for(int i = 0; i < gatesCount - 1; i++)
        {
            for (int j = 0; j < enemyCount/(gatesCount - 1); j++)
            {
                GameObject enemyChild = Instantiate(enemy);
                enemyChild.transform.position = enemyChild.transform.position + new Vector3(Random.RandomRange(-1.5f,1.5f), 0, Random.RandomRange(enemySpawnpoint_1, enemySpawnpoint_2));
                enemyChild.transform.parent = enemyParent.transform;
            }
            enemySpawnpoint_1 = enemySpawnpoint_2;
            enemySpawnpoint_2 = enemySpawnpoint_2 + valueHelperEnemy;
        }

    }

    
    private void LevelText()
    {
        level = PlayerPrefs.GetInt("level");
        levelText.text = "LEVEL " + (level + 1);
    }
    private void NextLevelCount()
    {
        level = PlayerPrefs.GetInt("level");
        PlayerPrefs.SetInt("level", level+1);
    }

    // return values
    public float FinishXCoin()
    {
        return xCoin = xCoin + 0.2f;
    }

    public int UpdateCoinNow(int value)
    {
        return nowCoin = nowCoin + value;
    }

    public int  SetEnemyLCoin()
    {
        return enemyLCoin;
    }
    public int SetEnemyLHealth()
    {
        return enemyLHealth;
    }
    public int SetEnemyCoin()
    {
        return enemyCoin;
    }
    public int SetEnemyHealth()
    {
        return enemyHealth;
    }

    public void FailPanelActive()
    {
        failPanel.SetActive(true);
    }
    public void WinPanelActive()
    {
        winPanel.SetActive(true);
    }
    public void StartPanelDeActive()
    {
        startPanel.SetActive(false);
    }
    public void ShopScene()
    {
        SceneManager.LoadScene("ShopUI");
    }
    public void NextLevelADS()
    {
        //ADS Code...
        GameDataManager.AddCoins(((int)(nowCoin * xCoin)));
        NextLevelCount();
        SceneManager.LoadScene("MainScreen");
    }
    public void NextLevel()
    {
        GameDataManager.AddCoins(nowCoin);
        NextLevelCount();
        SceneManager.LoadScene("MainScreen");
        
    }
    public void Restart()
    {
        //how to save screen and load ??
        SceneManager.LoadScene("MainScreen");
    }
}
