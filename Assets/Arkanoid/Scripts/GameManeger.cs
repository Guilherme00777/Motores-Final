using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    public static GameManeger instance;
    public int vidas = 3;
    public int tijolosRestantes;

    public GameObject playerPrefab;
    public GameObject ballPrefab;
    public Transform playerSpwnPoint;
    public Transform ballSpawnPoint;

    public PlayerB playerAtual;
    public BallB ballAtual;

    public TextMeshProUGUI contador;
    public TextMeshProUGUI msgFinal; 

    public bool segurando;
    private Vector3 offset;

   private void Awake()
   {
       instance = this;
   }
  
   void Start()
    {
        SpawnarNovoJogador();
        AtualizarContador();
        tijolosRestantes = GameObject.FindGameObjectsWithTag("Tijolo").Length;
    }

    public void AtualizarContador()
    {
      contador.text = $"vidas: {vidas}";
    }

    public void SpawnarNovoJogador()
    {
      GameObject playerObj = Instantiate(playerPrefab, playerSpwnPoint.position, Quaternion.identity); 
      GameObject ballObj = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity); 

      playerAtual = playerObj.GetComponent<PlayerB>();
      ballAtual = ballObj.GetComponent<BallB>();

      segurando = true;
      offset = playerAtual.transform.position - ballAtual.transform.position;


    }

    public void SubtrairTijolo()
    {
        tijolosRestantes--;

        if(tijolosRestantes<= 0)
        {
            Vitoria();
        }
    }

    public void SubtrairVida()
    {
        vidas--;
        AtualizarContador();
        Destroy(playerAtual.gameObject);
        Destroy(ballAtual.gameObject);
        if(vidas <= 0 )
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(SpawnarNovoJogador),  2);
        }
            
        
    }

    public void Vitoria()
    {
        msgFinal.text = "Parabéns";
        Destroy(ballAtual.gameObject);
        Invoke(nameof(ReiniciarCena),  2);
    }

    public void GameOver()
    {
       msgFinal.text = "Game Over";
       Invoke(nameof(ReiniciarCena),  2);
    }
    public void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    


   
    void Update()
    {
        if (segurando)
        {
            ballAtual.transform.position = playerAtual.transform.position - offset;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                ballAtual.DispararBolinha(playerAtual.inputX);
                segurando = false;
            }
        }
    }
}
