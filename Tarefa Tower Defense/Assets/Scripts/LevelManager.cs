using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    public static LevelManager instance; 

    public Transform startPoint;

    public Transform[] path;  
    [SerializeField] public int currency;  

    private void Awake()  
    {
        instance = this; 
    }

    private void Start() 
    {
        currency = 500;
    }

    public void IncreaseCurrency(int amount) 
    {
        amount = 50; 
        currency += amount; 
    }

    public bool SpendCurrency(int amount) 
    {
        if (amount <= currency) 
        {
            currency -= amount; 
            return true; 
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item"); 
            return false; 
        }
    }
}