using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player instance;

    public Equippable hand;
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

    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
    };

    private void Awake() {
        instance = this;

    }

    // Use this for initialization
    void Start() {
        myEnt = GetComponent<Entity>();
        ChangeEquippedItem(4);
        EquipHand();
        Inventory.Instance.ChangeToolBarSelected();
    }

    // Update is called once per frame
    public GameObject mega;
    void Update() {

        if (Input.GetKeyDown("v"))//Eat cheat button
        {
            Eat(20);
        }

        if (Input.GetKeyDown("b"))//Drink cheat button
        {
            Drink(20);
        }

        ChangeEquip();
        UpdateUIValues();
        InvokeSurvival();

        if (Input.GetKeyDown("m")) {
            Instantiate(mega, transform.position, transform.rotation);
        }

        //        Debug.Log(hunger);
    }

    private void UpdateUIValues() {

        if (hungerBar != null && thirstBar != null && healthBar != null) {
            hungerBar.fillAmount = hunger / 100F;
            thirstBar.fillAmount = thirst / 100F;
            healthBar.fillAmount = myEnt.hp / 100F;
            if (hunger <= 0 || thirst <= 0 || myEnt.hp <= 0) {
                UIManager.instance.EndGame();
            }
        } else {
            Debug.Log("Hunger/Thirst/Healthbar is null");
        }

    }

    private void InvokeSurvival() {
        if (!dorst) {
            StartCoroutine(GetThirsty());
        }

        if (!honger) {
            StartCoroutine(GetHungry());
        }
    }

    private void ChangeEquip() {

        for (int i = 0; i < keyCodes.Length; i++) {
            if (currentWeapon != null && !currentWeapon.beingUsed) {
                if (Input.GetKeyDown(keyCodes[i])) {
                    int numberPressed = i + 1;

                    Inventory.Instance.SelectedToolbarSlot = numberPressed - 1;

                    Inventory.Instance.ChangeToolBarSelected();

                    ChangeEquippedItem(Inventory.Instance.SelectedToolbarSlot);
                }

            }
        }
    }

    public void EquipHand() {
        Inventory.itemInHand = hand;
        //Debug.Log(Inventory.itemInHand);
    }

    public void ChangeEquippedItem(int i) { //Item's listIndex variable must be equal to its place in the item list in this script

        if (i < Inventory.Instance.toolBar.Count) {
            Inventory.Instance.SelectedToolbarSlot = i;
        }

        if (!currentWeapon.beingUsed) {

            if (Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem != null) {
                //Debug.Log(Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem);
                items[Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem.itemListIndex].SetActive(true);
                Inventory.itemInHand = Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem.equippable;
                currentWeapon = items[Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem.itemListIndex].GetComponent<Weapon>();
            } else {
                Inventory.itemInHand = hand;

                items[4].SetActive(true); //Hand index
                currentWeapon = items[4].GetComponent<Weapon>();
                foreach (GameObject n in items) {
                    if (n != items[4]) {
                        n.SetActive(false);
                    }
                }
            }

            foreach (GameObject n in items) {

                if (Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem != null) {
                    if (items.IndexOf(n)!= Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem.itemListIndex) {
                        n.SetActive(false);
                    }
                }

            }
        } else if (currentWeapon == null && Inventory.Instance.toolBar[Inventory.Instance.SelectedToolbarSlot].myItem == null) {
            Inventory.itemInHand = hand;

            items[4].SetActive(true); //Hand index
            currentWeapon = items[4].GetComponent<Weapon>();
            foreach (GameObject n in items) {
                if (n != items[4]) {
                    n.SetActive(false);
                }
            }
        }

    }

    public IEnumerator GetHungry() {
        honger = true;
        if (!ateRecently) {
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

        if (!drankRecently) {
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
        if (hunger < 100) {
            hunger += nutrition;
        }
        if (hunger > 100) {
            hunger = 100;
            ateRecently = true;
            if (!ate) {
                StartCoroutine(TimeUntilHungryAgain());
            }
        }

    }

    public void Drink(float nutrition) {
        if (thirst < 100) {
            thirst += nutrition;
        }
        if (thirst > 100) {
            thirst = 100;
            drankRecently = true;
            if (!drank) {
                StartCoroutine(TimeUntilThirstyAgain());
            }
        }

    }
}