using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Interactable interactionTarget;
    public void interactInput()
    {
        interact();
    }
    private void interact()
    {        
        if (interactionTarget != null)
        {
            // Capture the original pickup so new triggers (e.g., from DropPart) don't overwrite it
            var pickedTarget = interactionTarget;
            var wc = gameObject.GetComponent<WeaponController>();
            GameObject weaponObject = wc.weaponObject;

            switch (interactionTarget.type)
            {
                case InteractionType.Magazine:
                    {
                        dropPart(weaponObject.GetComponent<MagazinePart>()); //drop old part
                        Type newPartType = interactionTarget.component.GetType();
                        print($"picking up a {newPartType}"); //print type of new part
                        MagazinePart newPart = (MagazinePart)weaponObject.AddComponent(newPartType); //add new component
                        newPart.properties = ((MagazinePart)interactionTarget.component).properties; //copy properties from pickup
                        DestroyImmediate(interactionTarget.gameObject);
                        wc.AssembleWeapon();
                        break;
                    }
                case InteractionType.BossSpawner:
                    {
                        Debug.Log("I SPAWNED YOU !!");
                        break;
                    }
            }
        }
    }
    void dropPart(WeaponPart partToDrop)
    {
        GameObject newPickupObject = new GameObject($"{partToDrop.GetType()} pickup");
        newPickupObject.transform.position = transform.position; //position set to players position
        Interactable newInter = newPickupObject.AddComponent<Interactable>();
        //create a copy of the part to drop copmonent on the new to drop object.
        if (partToDrop is MagazinePart)
        {
            Type partType = partToDrop.GetType();
            MagazinePart newComp = (MagazinePart)newPickupObject.AddComponent(partType);
            newComp.properties = ((MagazinePart)partToDrop).properties;
            newInter.type = InteractionType.Magazine;
            newInter.component = newComp;
        }
        else if (partToDrop is BarrelPart)
        {
            Type partType = partToDrop.GetType();
            BarrelPart newComp = (BarrelPart)newPickupObject.AddComponent(partType);
            newComp.properties = ((BarrelPart)partToDrop).properties;
            newInter.type = InteractionType.Barrel;
            newInter.component = newComp;
        }
        else if (partToDrop is BasePart)
        {
            Type partType = partToDrop.GetType();
            BasePart newComp = (BasePart)newPickupObject.AddComponent(partType);
            newComp.properties = ((BasePart)partToDrop).properties;
            newInter.type = InteractionType.BasePart;
            newInter.component = newComp;
        }
        else if (partToDrop is GripPart)
        {
            Type partType = partToDrop.GetType();
            GripPart newComp = (GripPart)newPickupObject.AddComponent(partType);
            newComp.properties = ((GripPart)partToDrop).properties;
            newInter.type = InteractionType.Grip;
            newInter.component = newComp;
        }
        else if (partToDrop is StockPart)
        {
            Type partType = partToDrop.GetType();
            StockPart newComp = (StockPart)newPickupObject.AddComponent(partType);
            newComp.properties = ((StockPart)partToDrop).properties;
            newInter.type = InteractionType.Stock;
            newInter.component = newComp;
        }
        else
        {
            print("invalid part type");
        }
        DestroyImmediate(partToDrop);
    }
}
