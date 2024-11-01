using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Inst�ncia est�tica do BuildManager para acesso global
    public static BuildManager Instance;

    [Header("References")]
    [SerializeField] private Tower[] towers;  // Array de torres dispon�veis para constru��o
    private int selectedTower = 0;  // �ndice da torre atualmente selecionada

    private void Awake()
    {
        // Garante que exista apenas uma inst�ncia do BuildManager e a define como acess�vel globalmente
        Instance = this;
    }

    // M�todo que retorna a torre atualmente selecionada
    public Tower GetselectedTower()
    {
        return towers[selectedTower];  // Retorna a torre do array com base no �ndice selecionado
    }

    // M�todo para definir a torre selecionada
    public void SetSelectedTower(int _selectedtower)
    {
        selectedTower = _selectedtower;  // Atualiza o �ndice da torre selecionada
    }
}
