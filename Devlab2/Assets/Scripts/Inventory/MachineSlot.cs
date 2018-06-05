using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineSlot : Slot
{
    public Animator slotAnimator;
    public bool productSlot;
    public bool productDone;
    public Image fillImage;

    public override void MouseEnter()
    {
        slotAnimator.SetBool("Enter", true);
        base.MouseEnter();
    }

    public override void MouseExit()
    {
        slotAnimator.SetBool("Enter", false);
        base.MouseExit();
    }

    public override void SetItem(Item newItem)
    {
        base.SetItem(newItem);
        if (productSlot == false)
        {
            Interactor.instance.CheckResipe(((CraftMachine)Interactor.instance.myMachine).validItems);
        }
    }

    public override void StartItemDrag()
    {
        if (productSlot == false || productDone == true)
        {
            Debug.Log("s");
            base.StartItemDrag();
        }
    }

    public override void StopItemDrag()
    {
        if (productSlot == false)
        {
            base.StopItemDrag();
            return;
        }

        if (productDone == false)
        {
            return;
        }

        if (myItem != null)
        {
            if (Inventory.mouseOver != null)
            {
                if (Inventory.mouseOver.myItem != null)
                {
                    itemImage.gameObject.SetActive(true);
                }
                else if (Inventory.mouseOver.GetType() != typeof(CharacterSlot) || Inventory.mouseOver.GetType() == typeof(CharacterSlot))
                {
                    if (Inventory.mouseOver.GetType() != typeof(CraftingSlot))
                    {
                        productDone = false;
                        Item newitem = myItem;
                        Inventory.mouseOver.SetItem(Instantiate(newitem));
                        RemoveItem();
                        ((CraftMachine)Interactor.instance.myMachine).CheckRecipe();
                    }
                    else
                    {
                        itemImage.gameObject.SetActive(true);
                    }
                }
                if (Inventory.mouseOver.GetType() == typeof(CraftingSlot) || GetType() == typeof(CraftingSlot))
                {
                    ((CraftMachine)Interactor.instance.myMachine).CheckRecipe();
                }
            }
            else
            {
                itemImage.gameObject.SetActive(true);
            }
            Inventory.Instance.StopDrag();
        }
    }

    public IEnumerator Processing()
    {
        float ftimer = ((CraftMachine)Interactor.instance.myMachine).timeToConvert;
        float ttimer = ftimer;


        while (productDone == false)
        {
            ftimer -= Time.deltaTime;

            if (ftimer <= 0)
            {
                fillImage.fillAmount = 0;
                productDone = true;
            }
            else
            {
                fillImage.fillAmount = ftimer / ttimer;
            }
            yield return null;
        }
    }
}
