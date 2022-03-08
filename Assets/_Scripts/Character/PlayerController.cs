using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] Sprite _sprite;
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    public LayerMask grassLayer;
    public LayerMask fovLayer;


    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterEnnemisView;


    public Animator animator;

    private bool _isMoving;
    private Vector2 _input;


    public void HandleUpdate()
    {
        if (!_isMoving)
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");

            if (_input != Vector2.zero)
            {
                animator.SetFloat("moveX", _input.x);
                animator.SetFloat("moveY", _input.y);

                var targetPos = transform.position;
                targetPos.x += _input.x;
                targetPos.y += _input.y;
                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos, OnMoveOver));
            }
        }
        animator.SetBool("isMoving", _isMoving);

        if (Input.GetKeyDown(KeyCode.E))
            Interact();

    }

    private void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }


    IEnumerator Move(Vector3 targetPos, Action OnMoveOver = null)
    {
        _isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        _isMoving = false;

        OnMoveOver?.Invoke();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
    private void OnMoveOver()
    {
        CheckForEncouters();
        CheckIfInEnnemisView();
    }


    private void CheckForEncouters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                OnEncountered();
                animator.SetBool("isMoving", false);
            }
        }
    }

    private void CheckIfInEnnemisView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, fovLayer);
        if (collider != null)
        {
            animator.SetBool("isMoving", false);
            OnEnterEnnemisView?.Invoke(collider);
            animator.SetFloat("moveY", 1);
            animator.SetFloat("moveX", 0);
        }

    }

    public Sprite Sprite { get => _sprite; }
    public string Name { get => _name; }

}
