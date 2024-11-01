using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;  // Refer�ncia ao componente SpriteRenderer para alterar a cor do plot
    [SerializeField] private Color hoverColor;  // Cor que ser� aplicada quando o mouse estiver sobre o plot

    private GameObject tower;  // Armazena a inst�ncia da torre constru�da neste plot
    private Color startColor;  // Cor original do plot, armazenada para restaurar ap�s o mouse sair

    private void Start()
    {
        // Armazena a cor inicial do plot ao iniciar o jogo
        startColor = sr.color;
    }

    private void OnMouseDown()
    {
        // Verifica se j� existe uma torre neste plot
        if (tower != null) return;

        // Obt�m a torre selecionada do gerenciador de constru��o
        Tower towertobuild = BuildManager.Instance.GetselectedTower();

        // Verifica se o jogador tem moeda suficiente para construir a torre
        if (towertobuild.cost > LevelManager.instance.currency)
        {
            Debug.Log("you can't afford this");  // Mensagem de erro se o jogador n�o puder pagar
            return;
        }

        // Deduz o custo da torre da moeda do jogador
        LevelManager.instance.SpendCurrency(towertobuild.cost);
        // Instancia a torre na posi��o deste plot
        tower = Instantiate(towertobuild.prefab, transform.position, Quaternion.identity);
    }

    private void OnMouseEnter()
    {
        // Altera a cor do plot para a cor de hover quando o mouse entra
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        // Restaura a cor original do plot quando o mouse sai
        sr.color = startColor;
    }
}
