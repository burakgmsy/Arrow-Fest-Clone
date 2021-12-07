using UnityEngine;
using TMPro;


public class GateManager : MonoBehaviour
{
    public static GateManager instance;
    [SerializeField] private float moveLimit = 0.9f;
    [SerializeField] private float moveSpeed = 0.9f;
    [SerializeField] private int value;
    [SerializeField] public bool isMoving;
    [SerializeField] TMP_Text valueText;
    [SerializeField] TMP_Text operationText;
    [SerializeField] public GameObject gateScreen;
    [SerializeField] private Material[] materials;
    private Vector3 startPos;
    private Renderer rend;
    private int switchValue;
    


    private void Start()
    {
        startPos = transform.position;
        rend = gateScreen.GetComponent<Renderer>();
        rend.enabled = true;  //blue and red material need
        RandomCreate();
    }
    private void Update()
    {
        if (isMoving)  
            GateMoving();
    }


    public void RandomCreate()
    {

        switchValue = Random.Range(0, 4); //range 0,1,2,3,4
        switch (switchValue)
        {
            case 0:
                SumGate();
                break;

            case 1:
                MinusGate();
                break;

            case 2:
                MultiplyGate();
                break;
            case 3:
                DivideGate();
                break;

            default:
                Debug.Log("Sýkýntý var");
                break;
        }

    }

    public  void SumGate()//toplama
    {
        gameObject.tag = "sumGate";
        value = Random.Range(1, 30);
        valueText.text = value.ToString();
        operationText.text = "+";
        rend.sharedMaterial = materials[1];
    }
    public void MinusGate() //çýkarma
    {
        gameObject.tag = "minusGate";
        value = Random.Range(1, 30);
        valueText.text = value.ToString();
        operationText.text = "-";
        rend.sharedMaterial = materials[0];
    }
    public void MultiplyGate()//çarpma
    {
        gameObject.tag = "multiplyGate";
        value = Random.Range(1, 3);
        valueText.text = value.ToString();
        operationText.text = "x";
        rend.sharedMaterial = materials[1];
    }
    public void DivideGate() // bölme
    {
        gameObject.tag = "divideGate";
        value = Random.Range(1, 3);
        valueText.text = value.ToString();
        operationText.text = "/";
        rend.sharedMaterial = materials[0];
    }
    public int GetValue() //return gate value
    {
        return value;
    }
    private void GateMoving() // move loop 
    {
        Vector3 vector = startPos;
        vector.x += moveLimit * Mathf.Sin(Time.time * moveSpeed);
        transform.position = vector;

    }
}
