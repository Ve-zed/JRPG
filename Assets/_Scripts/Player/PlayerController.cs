using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;
    public Animator animator;
    
    private bool _isMoving;
    private Vector2 _input;


    private void Update()
    {
        if (!_isMoving)
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");
            //for remove diag movement
            //if (input.x != 0) input.y = 0;

            if(_input != Vector2.zero)
            {
                animator.SetFloat("moveX", _input.x);
                animator.SetFloat("moveY", _input.y);

                var targetPos = transform.position;
                targetPos.x += _input.x;
                targetPos.y += _input.y;
                if(IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }
            animator.SetBool("isMoving", _isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        _isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        _isMoving = false;

        CheckForEncouters();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void CheckForEncouters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if(Random.Range(1,101) <= 10)
            {
                Debug.Log("POKEMON");
            }
        }
    }

}
