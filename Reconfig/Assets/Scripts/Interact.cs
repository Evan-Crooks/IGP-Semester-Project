using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Interactable interactionTaget;
    public void interactInput()
    {
        interact();
    }
    private void interact()
    {
        if (interactionTaget != null)
        {
            print(interactionTaget.component);
            if (interactionTaget.type == InteractionType.Magazine)
            {
                GameObject weaponObject = gameObject.GetComponent<WeaponController>().weaponObject;
                // Remove existing MagazinePart or its descendants
                MagazinePart oldPart = weaponObject.GetComponent<MagazinePart>();
                if (oldPart != null)
                {
                    Destroy(oldPart);
                }
                // Add new part of the correct type
                System.Type partType = interactionTaget.component.GetType();
                MagazinePart newMagazinePart = (MagazinePart)weaponObject.AddComponent(partType);
                newMagazinePart.properties = ((MagazinePart)interactionTaget.component).properties;


                gameObject.GetComponent<WeaponController>().AssembleWeapon();
            }
        }
    }
}
