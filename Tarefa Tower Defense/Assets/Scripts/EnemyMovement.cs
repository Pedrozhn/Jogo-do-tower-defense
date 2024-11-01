using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb; // Rigidbody do inimigo para controlar a física
    [SerializeField] private float moveSpeed = 2f; // Velocidade de movimento do inimigo

    private Transform target; // Alvo atual que o inimigo está seguindo
    private int pathIndex = 0; // Índice atual do caminho
    private float baseSpeed; // Velocidade base do inimigo, para restaurar depois

    private void Start()
    {
        baseSpeed = moveSpeed; // Armazena a velocidade base para possíveis resets
        target = LevelManager.instance.path[pathIndex]; // Define o primeiro alvo do caminho
    }

    private void Update()
    {
        // Verifica se o inimigo chegou perto o suficiente do alvo
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++; // Avança para o próximo ponto do caminho

            // Se chegou ao final do caminho
            if (pathIndex == LevelManager.instance.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke(); // Notifica que o inimigo foi destruído
                Destroy(gameObject); // Remove o inimigo da cena
                return;
            }
            else
            {
                // Atualiza o alvo para o próximo ponto do caminho
                target = LevelManager.instance.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        // Calcula a direção para o alvo e normaliza o vetor
        Vector2 direction = (target.position - transform.position).normalized;
        // Move o inimigo na direção do alvo com a velocidade definida
        rb.velocity = direction * moveSpeed;
    }

    // Método para atualizar a velocidade do inimigo, por exemplo, para efeitos de lentidão
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed; // Atualiza a velocidade do inimigo
    }

    // Método para resetar a velocidade do inimigo para a velocidade base
    public void ResetSpeed()
    {
        moveSpeed = baseSpeed; // Restaura a velocidade original
    }
}
