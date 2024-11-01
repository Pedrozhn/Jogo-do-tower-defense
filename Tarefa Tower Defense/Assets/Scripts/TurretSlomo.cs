using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSlomo : Turret
{
    [SerializeField] private float aps = 4f;  // A quantidade de ataques por segundo da torre

    [SerializeField] private float FreezeTime = 1f;  // O tempo que os inimigos permanecer�o congelados

    private void Update()
    {
        RotateTowardsTarget();  // Rotaciona a torre em dire��o ao alvo
        timeUntilFire += Time.deltaTime;  // Acumula o tempo at� o pr�ximo ataque

        // Verifica se � hora de atacar com base na taxa de ataques por segundo
        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();  // Congela os inimigos ao alcance
            timeUntilFire = 0f;  // Reinicia o temporizador para o pr�ximo ataque
        }
    }

    private void FreezeEnemies()
    {
        // Realiza um cast circular para detectar inimigos dentro do alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);

        // Se houver inimigos detectados, aplica o efeito
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();  // Obt�m o script de movimento do inimigo
                Health enemyHealth = hit.transform.GetComponent<Health>();  // Obt�m o script de sa�de do inimigo

                // Aplica dano ao inimigo (ajuste o valor conforme necess�rio)
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(0.1f);
                }

                // Reduz a velocidade do inimigo
                em.UpdateSpeed(0.5f);
                StartCoroutine(ResetEnemySpeed(em));  // Inicia a coroutine para restaurar a velocidade
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(FreezeTime);  // Espera o tempo de congelamento
        em.ResetSpeed();  // Restaura a velocidade original do inimigo
    }

    private void RotateTowardsTarget()
    {
        // Verifica se h� um alvo para rotacionar
        if (target == null)
        {
            // Debug.LogWarning("Nenhum alvo detectado para rotacionar.");
            return;  // Se n�o houver alvo, sai do m�todo
        }

        // Calcula o �ngulo necess�rio para rotacionar a torre em dire��o ao alvo
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        // Cria a rota��o alvo a partir do �ngulo calculado
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        // Rotaciona a torre suavemente em dire��o ao alvo
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }
}
