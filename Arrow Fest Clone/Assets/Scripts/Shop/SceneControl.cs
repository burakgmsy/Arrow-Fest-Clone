using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void gameScene()
    {
        SceneManager.LoadScene("MainScreen");
    }
    public void shopScene()
    {
        SceneManager.LoadScene("ShopUI");
    }
}
