using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player instance;

    public List<GameObject> items = new List<GameObject>();
    public Weapon currentWeapon;
    public static int arrows = 50;

    private float hunger = 100; //100 = full 0 = ded
    public float hungerInterval;

    private float thirst = 100;
    public float thirstInterval;

    public bool ateRecently;
    public bool drankRecently;

    public Image hungerBar;
    public Image thirstBar;
    public Image healthBar;


    private bool dorst;
    private bool honger;

    private bool ate;
    private bool drank;

    private Entity myEnt;

    private void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        myEnt = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update() {

        if(Input.GetKeyDown("v")) //Eat cheat button
        {
            Eat(20);
        }

        if(Input.GetKeyDown("b")) //Drink cheat button
        {
            Drink(20);
        }

        ChangeEquip();
        UpdateUIValues();
        InvokeSurvival();

        //        Debug.Log(hunger);
    }

    private void UpdateUIValues() {

        hungerBar.fillAmount = hunger / 100F;
        thirstBar.fillAmount = thirst / 100F;
        healthBar.fillAmount = myEnt.hp / 100F;
    }

    private void InvokeSurvival() {
        if(!dorst) {
            StartCoroutine(GetThirsty());
        }

        if(!honger) {
            StartCoroutine(GetHungry());
        }
    }

    private void ChangeEquip() {
        if(Input.GetKeyDown("1")) {
            ChangeEquippedItem(0);
        }
        else if(Input.GetKeyDown("2")) {
            ChangeEquippedItem(1);
        }
        else if(Input.GetKeyDown("3")) {
            ChangeEquippedItem(2);
        }
        else if(Input.GetKeyDown("4")) {
            ChangeEquippedItem(3);
        }
        else if(Input.GetKeyDown("5")) {
            ChangeEquippedItem(4);
        }
        else if(Input.GetKeyDown("0")) {
            ChangeEquippedItem(99);
        }
    }

    public void ChangeEquippedItem(int i) {
        if(!currentWeapon.beingUsed) {
            foreach(GameObject g in items) {
                if(i == items.IndexOf(g)) {
                    g.SetActive(true);
                    Inventory.itemInHand = g.GetComponent<Weapon>().equippable;
                    currentWeapon = g.GetComponent<Weapon>();
                }
                else {
                    g.SetActive(false);
                }
            }
        }

    }

    public IEnumerator GetHungry() {
        honger = true;
        if(!ateRecently) {
            hunger--;
        }
        yield return new WaitForSeconds(hungerInterval);
        honger = false;
    }

    private IEnumerator TimeUntilHungryAgain() {
        ate = true;
        yield return new WaitForSeconds(420); //420 = 7 minutes
        ateRecently = false;
        ate = false;
    }

    public IEnumerator GetThirsty() {
        dorst = true;
        yield return new WaitForSeconds(thirstInterval);

        if(!drankRecently) {
            thirst--;
        }

        dorst = false;

    }

    private IEnumerator TimeUntilThirstyAgain() {
        yield return new WaitForSeconds(240); //4 minutes
        drankRecently = false;
        drank = false;
    }

    public void Eat(float nutrition) {
        if(hunger < 100) {
            hunger += nutrition;
        }
        if(hunger > 100) {
            hunger = 100;
            ateRecently = true;
            if(!ate) {
                StartCoroutine(TimeUntilHungryAgain());
            }
        }

    }

    public void Drink(float nutrition) {
        if(thirst < 100) {
            thirst += nutrition;
        }
        if(thirst > 100) {
            thirst = 100;
            drankRecently = true;
            if(!drank) {
                StartCoroutine(TimeUntilThirstyAgain());
            }
        }

    }
}
