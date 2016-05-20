using UnityEngine;
using System.Collections;

public class PunchAbility : Ability
{
    public Animator animationController;

    void Start()
    {
        animationController = GetComponent<Animator>();
    }
    void Update()
    {
        if (Activate())
        {
            animationController.SetTrigger("Punch");
            GetComponent<PlayerController>().hitSpire();
        }
    }
}
