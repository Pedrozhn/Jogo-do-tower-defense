using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface que define um comportamento para entidades que podem atacar
public interface Iatacavel
{
    // Método que deve ser implementado por classes que herdam esta interface
    void Atacar();
}
