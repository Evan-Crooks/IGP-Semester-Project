using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractionType type;
    public Component component;

    void OnTriggerEnter2D(Collider2D collision)
    {
        print($"trigger f{collision.gameObject.name}");
        if (collision.tag == "Player")
        {
            collision.GetComponent<Interact>().interactionTaget = this;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<Interact>().interactionTaget == this) collision.GetComponent<Interact>().interactionTaget = null;
        }
    }
}

public enum InteractionType
{
    BasePart,
    Barrel,
    Magazine,
    Stock,
    Grip
}