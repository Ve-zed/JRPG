using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] Character _selectedCharacter;

    public Material MaterialOutline;
    public Material MaterialDefault;
    public GameUI UI;

    private SelectionMode _currentMode;


    enum SelectionMode
    {
        Default,
        Attack
    } 


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D raycastHit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (raycastHit2D.collider != null)
            {
                Character character = raycastHit2D.collider.gameObject.GetComponent<Character>();
                if (character != null)
                {
                    if (_selectedCharacter != null)
                        _selectedCharacter.Visual.material = MaterialDefault;

                    _selectedCharacter = character;

                    character.Visual.material = MaterialOutline;
                    UI.SetCharacter(character);
                }
                else
                {
                    _selectedCharacter.Attack(character);
                }
            }
        }
    }




    public void SetAttackMode()
    {
        if (_selectedCharacter == null)
            return;

        _currentMode = SelectionMode.Attack;
    }


}
