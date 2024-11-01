using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Instância estática do BuildManager para acesso global
    public static BuildManager Instance;

    [Header("References")]
    [SerializeField] private Tower[] towers;  // Array de torres disponíveis para construção
    private int selectedTower = 0;  // Índice da torre atualmente selecionada

    private void Awake()
    {
        // Garante que exista apenas uma instância do BuildManager e a define como acessível globalmente
        Instance = this;
    }

    // Método que retorna a torre atualmente selecionada
    public Tower GetselectedTower()
    {
        return towers[selectedTower];  // Retorna a torre do array com base no índice selecionado
    }

    // Método para definir a torre selecionada
    public void SetSelectedTower(int _selectedtower)
    {
        selectedTower = _selectedtower;  // Atualiza o índice da torre selecionada
    }
}
