using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int Life;
    public int LifeMax;

    public Sprite SpritePortrait;
    public SpriteRenderer Visual;

    public Animator Animator;

    internal void Attack(Character defender)
    {

        //Animator.SetTrigger("attack");
        defender.Hit();
    }
    internal void Hit()
    {
        //Animator.SetTrigger("hit");
    }

}
