using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Classe base para torres que implementam a interface Iatacavel
public class Turret : MonoBehaviour, Iatacavel
{
    [SerializeField] public Transform turretRotationPoint;  // Ponto de rotação da torre
    [SerializeField] protected float targetingrange = 5f;  // Alcance de ataque da torre
    [SerializeField] protected LayerMask enemyMask;  // Máscara para identificar inimigos
    [SerializeField] protected GameObject bulletPrefab;  // Prefab da bala que a torre dispara
    [SerializeField] protected Transform firingPoint;  // Ponto de onde a bala será disparada
    [SerializeField] public float rotationspeed = 10f;  // Velocidade de rotação da torre
    [SerializeField] private float bps = 1f;  // Balas por segundo

    protected Transform target;  // Referência para o alvo atual
    protected float timeUntilFire;  // Temporizador para controlar o tempo até o próximo disparo

    // Método virtual para atacar, pode ser sobrescrito em subclasses
    public virtual void Atacar()
    {
        // Implementação deixada em branco para ser definida nas subclasses
    }

    /* 
    // (Opcional) Método para desenhar gizmos no editor, útil para visualizar o alcance de ataque
    private void OnDrawGizmosSelected() 
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingrange);
    }
    */

    private void Update()
    {
        // Se não houver um alvo, tenta encontrá-lo
        if (target == null)
        {
            Findtarget();
            return;
        }

        // Rotaciona a torre em direção ao alvo
        RotateTowardsTarget();

        // Verifica se o alvo ainda está dentro do alcance
        if (!checktargetisrange())
        {
            target = null;  // Se não estiver, limpa o alvo
        }
        else
        {
            // Acumula o tempo até o próximo disparo
            timeUntilFire += Time.deltaTime;

            // Verifica se é hora de disparar
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();  // Dispara uma bala
                timeUntilFire = 0f;  // Reinicia o temporizador
            }
        }
    }

    private void RotateTowardsTarget()
    {
        // Se não houver alvo, exibe um aviso e sai do método
        if (target == null)
        {
            Debug.LogWarning("Nenhum alvo detectado para rotacionar.");
            return;
        }

        // Calcula o ângulo para rotacionar em direção ao alvo
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        // Cria a rotação alvo a partir do ângulo calculado
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        // Rotaciona a torre suavemente em direção ao alvo
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }

    protected virtual void Shoot()
    {
        // Instancia uma nova bala na posição do ponto de disparo
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);

        // Obtém o script da bala do objeto instanciado
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        // Define o alvo da bala
        bulletScript.SetTarget(target);
    }

    private bool checktargetisrange()
    {
        // Verifica se o alvo está dentro do alcance
        return Vector2.Distance(target.position, transform.position) <= targetingrange;
    }

    private void Findtarget()
    {
        // Realiza um cast circular para detectar inimigos dentro do alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);

        // Se houver inimigos detectados, define o primeiro como alvo
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
}
