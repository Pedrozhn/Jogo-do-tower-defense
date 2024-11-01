using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;  // Array de prefabs de inimigos que podem ser gerados

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;  // N�mero base de inimigos por onda
    [SerializeField] private float enemiesPerSecond = 0.5f;  // Taxa de gera��o de inimigos (inimigos por segundo)
    [SerializeField] private float timebetweenWaves = 5f;  // Tempo de espera entre ondas de inimigos
    [SerializeField] private float difficultyScallingFactor = 0.75f;  // Fator para escalar a dificuldade

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();  // Evento chamado quando um inimigo � destru�do

    private int currentwave = 1;  // N�mero da onda atual
    private float timesinceLastSpawn;  // Temporizador para controlar a gera��o de inimigos
    private int enemiesAlive;  // N�mero de inimigos ainda vivos
    private int enemiesLeftToSpawn;  // N�mero de inimigos restantes a serem gerados
    private bool isSpawning = false;  // Flag para indicar se os inimigos est�o sendo gerados

    private void Awake()
    {
        // Adiciona o m�todo EnemyDestroyed ao evento onEnemyDestroy
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        // Inicia a primeira onda de inimigos
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        // Se n�o estiver gerando inimigos, sai do m�todo
        if (!isSpawning) return;

        // Atualiza o temporizador desde o �ltimo inimigo gerado
        timesinceLastSpawn += Time.deltaTime;

        // Verifica se � hora de gerar um novo inimigo
        if (timesinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();  // Gera um inimigo
            enemiesLeftToSpawn--;  // Reduz o n�mero de inimigos restantes
            enemiesAlive++;  // Aumenta o n�mero de inimigos vivos
            timesinceLastSpawn = 0f;  // Reinicia o temporizador
        }

        // Verifica se todos os inimigos foram gerados e destru�dos
        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0)
        {
            Endwave();  // Finaliza a onda
        }
    }

    private void Endwave()
    {
        // Finaliza a onda atual e inicia a pr�xima
        isSpawning = false;
        timesinceLastSpawn = 0f;
        currentwave++;  // Aumenta o n�mero da onda
        StartCoroutine(StartWave());  // Inicia a pr�xima onda
    }

    private void EnemyDestroyed()
    {
        // Reduz o n�mero de inimigos vivos ao destruir um
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        // Espera o tempo definido antes de iniciar a gera��o de inimigos
        yield return new WaitForSeconds(timebetweenWaves);
        isSpawning = true;  // Marca que a gera��o de inimigos come�ou
        enemiesLeftToSpawn = EnemiesPerwave();  // Define quantos inimigos gerar nesta onda
    }

    private void SpawnEnemy()
    {
        // Seleciona aleatoriamente um prefab de inimigo do array e o instancia na posi��o inicial
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerwave()
    {
        // Calcula quantos inimigos devem ser gerados nesta onda com base na dificuldade
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentwave, difficultyScallingFactor));
    }
}
