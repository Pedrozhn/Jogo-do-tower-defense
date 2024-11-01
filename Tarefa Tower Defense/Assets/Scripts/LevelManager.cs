using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Instância estática do LevelManager para acesso global
    public static LevelManager instance;

    public Transform startPoint;  // Ponto de início para os inimigos ou torres
    public Transform[] path;  // Array de pontos que definem o caminho dos inimigos
    [SerializeField] public int currency;  // Quantidade de moeda do jogador

    private void Awake()
    {
        // Garante que exista apenas uma instância do LevelManager e a define como acessível globalmente
        instance = this;
    }

    private void Start()
    {
        // Inicializa a quantidade de moeda do jogador
        currency = 500;
    }

    // Método para aumentar a quantidade de moeda do jogador
    public void IncreaseCurrency(int amount)
    {
        amount = 50;  // Define um valor fixo para o aumento (ajustar se necessário)
        currency += amount;  // Adiciona o valor ao total de moeda
    }

    // Método para gastar moeda, retorna true se a transação for bem-sucedida
    public bool SpendCurrency(int amount)
    {
        // Verifica se o jogador tem moeda suficiente
        if (amount <= currency)
        {
            currency -= amount;  // Deduz o valor da moeda
            return true;  // Transação bem-sucedida
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item");  // Mensagem de erro se não houver moeda suficiente
            return false;  // Transação falhou
        }
    }
}
