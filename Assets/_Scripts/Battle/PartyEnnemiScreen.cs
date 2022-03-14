using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyEnnemiScreen : MonoBehaviour
{
    private BattleHud[] _memberSlots;
    private List<Monster> _monsters;
    private Image _image;

    public void Init()
    {
        _memberSlots = GetComponentsInChildren<BattleHud>(true);
    }
    public void SetPartyData(List<Monster> monsters)
    {
        //this.gameObject.SetActive(true);
        _monsters = monsters;

        for (int i = 0; i < _memberSlots.Length; i++)
        {
            if (i < monsters.Count)
            {
                _memberSlots[i].SetData(monsters[i]);
            }
            else
                _memberSlots[i].gameObject.SetActive(false);
        }
    }


    /*public void UpdateSelectedMember(int selectedMember)
    {
        for (int i = 0; i < _monsters.Count; i++)
        {
            if (i == selectedMember)
            {
                _memberSlots[i].SetSelected(true);
            }
            else
                _memberSlots[i].SetSelected(false);
        }
    }*/
    public void SetMessageText(string message)
    {
        //_messageText.text = message;
    }

}
