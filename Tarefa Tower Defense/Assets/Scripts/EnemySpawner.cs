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
    [SerializeField] private int baseEnemies = 8;  // Número base de inimigos por onda
    [SerializeField] private float enemiesPerSecond = 0.5f;  // Taxa de geração de inimigos (inimigos por segundo)
    [SerializeField] private float timebetweenWaves = 5f;  // Tempo de espera entre ondas de inimigos
    [SerializeField] private float difficultyScallingFactor = 0.75f;  // Fator para escalar a dificuldade

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();  // Evento chamado quando um inimigo é destruído

    private int currentwave = 1;  // Número da onda atual
    private float timesinceLastSpawn;  // Temporizador para controlar a geração de inimigos
    private int enemiesAlive;  // Número de inimigos ainda vivos
    private int enemiesLeftToSpawn;  // Número de inimigos restantes a serem gerados
    private bool isSpawning = false;  // Flag para indicar se os inimigos estão sendo gerados

    private void Awake()
    {
        // Adiciona o método EnemyDestroyed ao evento onEnemyDestroy
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        // Inicia a primeira onda de inimigos
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        // Se não estiver gerando inimigos, sai do método
        if (!isSpawning) return;

        // Atualiza o temporizador desde o último inimigo gerado
        timesinceLastSpawn += Time.deltaTime;

        // Verifica se é hora de gerar um novo inimigo
        if (timesinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();  // Gera um inimigo
            enemiesLeftToSpawn--;  // Reduz o número de inimigos restantes
            enemiesAlive++;  // Aumenta o número de inimigos vivos
            timesinceLastSpawn = 0f;  // Reinicia o temporizador
        }

        // Verifica se todos os inimigos foram gerados e destruídos
        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0)
        {
            Endwave();  // Finaliza a onda
        }
    }

    private void Endwave()
    {
        // Finaliza a onda atual e inicia a próxima
        isSpawning = false;
        timesinceLastSpawn = 0f;
        currentwave++;  // Aumenta o número da onda
        StartCoroutine(StartWave());  // Inicia a próxima onda
    }

    private void EnemyDestroyed()
    {
        // Reduz o número de inimigos vivos ao destruir um
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        // Espera o tempo definido antes de iniciar a geração de inimigos
        yield return new WaitForSeconds(timebetweenWaves);
        isSpawning = true;  // Marca que a geração de inimigos começou
        enemiesLeftToSpawn = EnemiesPerwave();  // Define quantos inimigos gerar nesta onda
    }

    private void SpawnEnemy()
    {
        // Seleciona aleatoriamente um prefab de inimigo do array e o instancia na posição inicial
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerwave()
    {
        // Calcula quantos inimigos devem ser gerados nesta onda com base na dificuldade
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentwave, difficultyScallingFactor));
    }
}
