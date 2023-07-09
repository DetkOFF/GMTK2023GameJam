using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class CashManager
{
    private float _balance;

    public CashManager(float startingBalance)
    {
        _balance = startingBalance;
    }

    public void AddToBalance(float ammount)
    {
        _balance += ammount;
    }

    public void RemoveFromBalance(float ammount)
    {
        if (_balance >= 0)
            _balance -= ammount;
        else
            Debug.Log("game failed");
    }
    public float GetBalance()
    {
        return _balance;
    }
    public void ShowBalance()
    {
        Debug.Log(_balance);
    }
}
