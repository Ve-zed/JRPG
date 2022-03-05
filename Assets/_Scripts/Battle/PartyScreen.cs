using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text _messageText;

    PartyMemberUI[] _memberSlots;

    List<Monster> _monsters;
    public void Init()
    {
        _memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }
    public void SetPartyData(List<Monster> monsters)
    {
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

        _messageText.text = "Choose a monster pls";

    }

    public void UpdateSelectedMember(int selectedMember)
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
    }
    public void SetMessageText(string message)
    {
        _messageText.text = message;
    }

}
