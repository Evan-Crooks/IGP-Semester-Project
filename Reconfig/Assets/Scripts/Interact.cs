using System.Collections;
using System.Collections.Generic;
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
            var wc = gameObject.GetComponent<WeaponController>();
            GameObject weaponObject = wc.weaponObject;

            switch (interactionTarget.type)
            {
                case InteractionType.BasePart:
                    {
                        BasePart old = weaponObject.GetComponent<BasePart>();
                        if (old != null) Destroy(old);
                        System.Type t = interactionTarget.component.GetType();
                        BasePart added = (BasePart)weaponObject.AddComponent(t);
                        added.properties = ((BasePart)interactionTarget.component).properties;
                        break;
                    }
                case InteractionType.Barrel:
                    {
                        BarrelPart old = weaponObject.GetComponent<BarrelPart>();
                        if (old != null) Destroy(old);
                        System.Type t = interactionTarget.component.GetType();
                        BarrelPart added = (BarrelPart)weaponObject.AddComponent(t);
                        added.properties = ((BarrelPart)interactionTarget.component).properties;
                        break;
                    }
                case InteractionType.Magazine:
                    {
                        MagazinePart old = weaponObject.GetComponent<MagazinePart>();
                        if (old != null) Destroy(old);
                        System.Type t = interactionTarget.component.GetType();
                        MagazinePart added = (MagazinePart)weaponObject.AddComponent(t);
                        added.properties = ((MagazinePart)interactionTarget.component).properties;
                        break;
                    }
                case InteractionType.Stock:
                    {
                        StockPart old = weaponObject.GetComponent<StockPart>();
                        if (old != null) Destroy(old);
                        System.Type t = interactionTarget.component.GetType();
                        StockPart added = (StockPart)weaponObject.AddComponent(t);
                        added.properties = ((StockPart)interactionTarget.component).properties;
                        break;
                    }
                case InteractionType.Grip:
                    {
                        GripPart old = weaponObject.GetComponent<GripPart>();
                        if (old != null) Destroy(old);
                        System.Type t = interactionTarget.component.GetType();
                        GripPart added = (GripPart)weaponObject.AddComponent(t);
                        added.properties = ((GripPart)interactionTarget.component).properties;
                        break;
                    }
            }

            wc.AssembleWeapon();
        }
    }
    void DropPart(WeaponPart part)
    {
        if (part == null) return;
        var wc = gameObject.GetComponent<WeaponController>();
        if (wc == null) return;

        InteractionType type;
        if (part is BasePart) type = InteractionType.BasePart;
        else if (part is BarrelPart) type = InteractionType.Barrel;
        else if (part is MagazinePart) type = InteractionType.Magazine;
        else if (part is StockPart) type = InteractionType.Stock;
        else if (part is GripPart) type = InteractionType.Grip;
        else return;

        GameObject go = new GameObject($"{type} Pickup");
        go.transform.position = transform.position + transform.right * 0.5f;

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = part.sprite;

        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        var col = go.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        System.Type concreteType = part.GetType();
        var newComp = go.AddComponent(concreteType);

        if (newComp is WeaponPart wpDst && part is WeaponPart wpSrc)
        {
            wpDst.sprite = wpSrc.sprite;
        }
        if (newComp is BasePart bpDst && part is BasePart bpSrc)
        {
            bpDst.properties = bpSrc.properties;
        }
        else if (newComp is BarrelPart blDst && part is BarrelPart blSrc)
        {
            blDst.properties = blSrc.properties;
        }
        else if (newComp is MagazinePart mgDst && part is MagazinePart mgSrc)
        {
            mgDst.properties = mgSrc.properties;
            mgDst.projectileSprite = mgSrc.projectileSprite;
        }
        else if (newComp is StockPart stDst && part is StockPart stSrc)
        {
            stDst.properties = stSrc.properties;
        }
        else if (newComp is GripPart gpDst && part is GripPart gpSrc)
        {
            gpDst.properties = gpSrc.properties;
        }

        var interactable = go.AddComponent<Interactable>();
        interactable.type = type;
        interactable.component = (Component)newComp;

        Destroy(part);
        wc.AssembleWeapon();
    }
}
