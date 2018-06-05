using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactor : MonoBehaviour
{
    public static Interactor instance;

    public Machine myMachine;
    public TextMeshProUGUI machineName;
    public Image machineImage;

    [Header("Basic")]
    public Image basicMachineUseImage;
    public GameObject Basic;

    #region Crafter
    [Header("Crafter")]
    public List<MachineSlot> craftSlots = new List<MachineSlot>();
    public List<MachineSlot> productSlots = new List<MachineSlot>();
    public Image crafterMachineUseImage;
    public GameObject crafter;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    public void ChangeInteractor(Machine newMachine)
    {
        myMachine = newMachine;

        machineImage.sprite = myMachine.machineImage;
        machineName.text = myMachine.machineName;

        Basic.SetActive(false);
        crafter.SetActive(false);

        if (myMachine.GetType() == typeof(CraftMachine))
        {
            crafter.SetActive(true);
            crafterMachineUseImage.sprite = myMachine.machineImage;
        }
        else
        {
            Basic.SetActive(true);
            basicMachineUseImage.sprite = myMachine.machineImage;
        }
    }

    public void CheckResipe(List<CraftMachine.Resipe> validItems)
    {
        if (myMachine.isTurnedOn == false)
        {
            return;
        }

        for (int i = 0; i < craftSlots.Count; i++)
        {
            for (int ii = 0; ii < validItems.Count; ii++)
            {
                if (craftSlots[i].myItem != null && craftSlots[i].myItem.itemName == validItems[ii].ingredient.itemName)
                {
                    for (int x = 0; x < productSlots.Count; x++)
                    {
                        if (productSlots[x].myItem == null)
                        {
                            productSlots[x].SetItem(validItems[ii].product);
                            StartCoroutine(productSlots[x].Processing());
                            craftSlots[i].RemoveItem();
                            break;
                        }
                        else if (x == productSlots.Count - 1)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
