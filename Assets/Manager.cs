using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public int p1Wins = 0;
    public int p2Wins = 0;
    [SerializeField] public GameObject win;
    public static Manager Instance;
    public bool outGame = true;
    public bool gameOver = false;
    void Awake()
    {
       
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Instance.outGame = true;
            Instance.gameOver = false;
            Instance.win = GameObject.Find("WinBackground");
            Destroy(gameObject);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit") && outGame == true)
        {
            outGame = false;
            win.transform.GetChild(0).GetComponent<Text>().text = "";
            win.SetActive(false);
            if (gameOver == true)
            {
                gameOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
