using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb; // Rigidbody do inimigo para controlar a f�sica
    [SerializeField] private float moveSpeed = 2f; // Velocidade de movimento do inimigo

    private Transform target; // Alvo atual que o inimigo est� seguindo
    private int pathIndex = 0; // �ndice atual do caminho
    private float baseSpeed; // Velocidade base do inimigo, para restaurar depois

    private void Start()
    {
        baseSpeed = moveSpeed; // Armazena a velocidade base para poss�veis resets
        target = LevelManager.instance.path[pathIndex]; // Define o primeiro alvo do caminho
    }

    private void Update()
    {
        // Verifica se o inimigo chegou perto o suficiente do alvo
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++; // Avan�a para o pr�ximo ponto do caminho

            // Se chegou ao final do caminho
            if (pathIndex == LevelManager.instance.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke(); // Notifica que o inimigo foi destru�do
                Destroy(gameObject); // Remove o inimigo da cena
                return;
            }
            else
            {
                // Atualiza o alvo para o pr�ximo ponto do caminho
                target = LevelManager.instance.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        // Calcula a dire��o para o alvo e normaliza o vetor
        Vector2 direction = (target.position - transform.position).normalized;
        // Move o inimigo na dire��o do alvo com a velocidade definida
        rb.velocity = direction * moveSpeed;
    }

    // M�todo para atualizar a velocidade do inimigo, por exemplo, para efeitos de lentid�o
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed; // Atualiza a velocidade do inimigo
    }

    // M�todo para resetar a velocidade do inimigo para a velocidade base
    public void ResetSpeed()
    {
        moveSpeed = baseSpeed; // Restaura a velocidade original
    }
}
