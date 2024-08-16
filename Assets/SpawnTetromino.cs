using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public static SpawnTetromino Instance;

    public GameObject[] Tetrominoes;
    public GameObject gameoverobject;
    public GameObject menuobject;

    private void Awake()
    {
        Instance= this;
    }
    void Start()
    {
        Newtetromiono();
        Time.timeScale= 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;

            rota rota = FindObjectOfType<rota>();
            if (rota != null)
            {
                rota.enabled = false;
            }
            menuobject.SetActive(true);
        }
    }
    public void Newtetromiono()
    {
        Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
    }
    public void Gameover()
    {
        Time.timeScale= 0f;

        rota rota = FindObjectOfType<rota>();
        if(rota != null)
        {
            rota.enabled= false;
        }
        menuobject.SetActive(true);
        gameoverobject.SetActive(true);
    }
    public void Newgame()
    {
        Debug.Log("Newgame triggered");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
