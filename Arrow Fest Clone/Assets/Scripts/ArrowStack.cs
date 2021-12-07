using System.Collections;
using TMPro;
using UnityEngine;


public class ArrowStack : Stack
{
    [SerializeField] float speed = 10f;
    [SerializeField] private float xBound = 1.5f;
    [SerializeField] public bool inFinishLine = false;
    [SerializeField] TMP_Text counterText;
    [SerializeField] GameObject counterTextObj;
    [SerializeField] Camera myCamera;
    [SerializeField] private GameObject controlHelper;
    [SerializeField] Pool[] arrowPrefab;
    GameManager _GameManager;
    BoxCollider boxCollider;
    public const float DISTANCEOFORBITS = 0.08f;
    const float PLANESIZE = 1.6f;
    string poolType;
    private int gateValue;
    int orbit = 1;
    private int level;
    private int selectedItem;
    private bool moving = false;

    void Start()
    {
        selectedItem = PlayerPrefs.GetInt("SelectedItem");// selected item in shop
        boxCollider = GetComponent<BoxCollider>();
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PoolSystem.ResetPools();
        AddNewStack();
        poolType = arrowPrefab[selectedItem].GetPoolType();
    }


    void FixedUpdate()
    {
        Move();
    }
    public void Move() // foward move
    {
        if (moving)
        {
            transform.position += (Vector3.forward * speed ) * Time.deltaTime;
            _GameManager.StartPanelDeActive();        
        }
        PlayerPosition(); //limited left and right
    }
    public void StartGame()
    {
        moving = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        //other options
        //gateValue = int.Parse(other.transform.GetChild(0).transform.GetComponent<TMP_Text>().text);
        //gateValue = other.gameObject.GetComponent<GateManager>().GetValue(); 

        if (other.gameObject.CompareTag("sumGate"))
        {
            
            gateValue = other.gameObject.GetComponent<GateManager>().GetValue();
            AddNewStack(gateValue);
        }

        if (other.gameObject.CompareTag("minusGate"))
        {
            //gateValue = int.Parse(other.transform.GetChild(0).transform.GetComponent<TMP_Text>().text);
            gateValue = other.gameObject.GetComponent<GateManager>().GetValue();
            RemoveArrows(gateValue);
        }

        if (other.gameObject.CompareTag("multiplyGate"))
        {
            gateValue = int.Parse(other.transform.GetChild(0).transform.GetComponent<TMP_Text>().text);
            //gateValue = other.gameObject.GetComponent<GateManager>().GetValue(); 
            if (gateValue > 1)
                AddNewStack((gateValue * StackCount)- StackCount);
        }
        if (other.gameObject.CompareTag("divideGate"))
        {
            //gateValue = int.Parse(other.transform.GetChild(0).transform.GetComponent<TMP_Text>().text);
            gateValue = other.gameObject.GetComponent<GateManager>().GetValue();
            if (gateValue > 1)
                RemoveArrows(StackCount/gateValue);
        }
        if (other.gameObject.CompareTag("inFinishLine"))
        {
            
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, 150f, 2f * Time.deltaTime);
            speed = 12f;
            inFinishLine = true;
            SetAllArrowScale(0.01f, 0.05f);
            counterTextObj.SetActive(false);
            //**todo controller disable & arrowstack  x axis = 0  

        }
        if (other.gameObject.CompareTag("Finish"))
        {
            level = PlayerPrefs.GetInt("level");
            level = level + 1;
            PlayerPrefs.SetInt("level", level);
            _GameManager.WinPanelActive();

        }
        if (other.gameObject.CompareTag("FinishXPanel"))
        {
            Debug.Log("test");
            _GameManager.FinishXCoin();
        }
        SetAllArrowsLocation(Mathf.Clamp((PLANESIZE - Mathf.Abs(transform.position.x)), 0.3f, 1f) / PLANESIZE, 1f);
        SetArrowCounterText();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            GameDataManager.AddCoins(_GameManager.SetEnemyCoin());
            RemoveArrows(_GameManager.SetEnemyHealth());
            if (inFinishLine)
            {
                _GameManager.UpdateCoinNow(_GameManager.SetEnemyCoin());
            }
        }
        if (collision.gameObject.CompareTag("EnemyL"))
        {
            GameDataManager.AddCoins(_GameManager.SetEnemyLCoin());
            RemoveArrows(_GameManager.SetEnemyLHealth());
            if (inFinishLine)
            {
                _GameManager.UpdateCoinNow(_GameManager.SetEnemyLCoin());
            }
        }
    }
    public void AddNewStack(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newArrow = arrowPrefab[selectedItem].InstantiniatePool();
            newArrow.transform.parent = transform;
            AddStack(newArrow);
            newArrow.transform.localPosition = GetNewLocationForNewArrow(StackCount);
            newArrow.GetComponent<Arrow>().SetValues(orbit);
            if ((orbit + 2) * (orbit + 3) < StackCount)
                orbit++;
        }
    }
    public void RemoveArrows(int count, bool lostControl = true)
    {
        for (int i = 0; i < count; i++)
        {
            if (ControlLost(lostControl))
                return;
            GetAnArrow().DestroyPool();
            if ((orbit + 2) * (orbit + 1) > StackCount)
                orbit--;
        }
    }
    public bool ControlLost(bool lostControl = true)
    {
        if (StackCount <= 0)
        {
            if (lostControl)
                if (inFinishLine)
                    _GameManager.WinPanelActive();
                else
                _GameManager.FailPanelActive();
                speed = 0;
            return true;
        }
        return false;
    }
    public Vector3 GetNewLocationForNewArrow(int index)
    {
        float _index = ((index) - ((orbit + 1) * (orbit + 2)));
        float radian;
        if (_index != 0)
            radian = 2 * Mathf.PI / ((orbit + 3) * (orbit + 2) - (orbit + 1) * (orbit + 2)) * _index;
        else
            radian = 0;
        Vector3 spawnDir = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);
        return spawnDir * orbit * DISTANCEOFORBITS;
    }
    public void SetAllArrowsLocation(float x, float y)
    {
        foreach (Arrow item in stackObjects)
        {
            item.SetLocation(x, y);
        }
    }
    public void SetAllArrowScale(float x,float y)
    {
        foreach (Arrow item in stackObjects)
        {
            item.SetScale(x, y);
        }
    }
    public Arrow GetAnArrow()
    {
        Arrow arrow = GetLastStack() as Arrow;
        arrow.transform.parent = null;
        RemoveStack();
        SetArrowCounterText();
        return arrow;
    }
    public void SetArrowCounterText()
    {
        counterText.text = StackCount.ToString();
    }
    private void OnDestroy() //Gereksiz gibi 
    {
        //TouchSystem.Touching -= TouchSystem_Touching;
    }
    private void PlayerPosition()
    {
        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
            
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
            
        }
        ArrowStackScale();
    }
    private void ArrowStackScale()
    {
        if (transform.position.x > 1 || transform.position.x < -1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.4f, 1, 1), speed * Time.deltaTime);
        }
        if (inFinishLine)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(4f, 0.5f, 1), speed * Time.deltaTime);
            boxCollider.size = new Vector3(1f, 0.3f, 0.4f);

        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), speed * Time.deltaTime);
        }
    }
}


