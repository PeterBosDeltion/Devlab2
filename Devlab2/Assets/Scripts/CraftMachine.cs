using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMachine : Machine {
    public List<Resipe> validItems = new List<Resipe>();

    public float timeToConvert;
    public float fuelTimer;

    private void Start() {
        Interactor.instance.ChangeInteractor(this);
    }

    public void CheckRecipe() {
        Interactor.instance.CheckResipe(validItems);
    }

    public void TurnOn() {
        CheckRecipe();
    }

    public void TurnOff() {

    }

    [System.Serializable]
    public struct Resipe {
        public Item product;
        public Item ingredient;
    }
}
