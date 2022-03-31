using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_DataCenter : MonoBehaviour
{
    [SerializeField] GameObject dataCenter;
    [SerializeField] GameObject treeNFT;
    [SerializeField] GameObject btnTreeNFT;
    [SerializeField] GameObject options;
    [SerializeField] GameObject btnOptions;
    [SerializeField] GameObject tree_Player;
    [SerializeField] GameObject btnPlayer;
    [SerializeField] GameObject tree_Partner_1;
    [SerializeField] GameObject btnPartner_1;
    [SerializeField] GameObject tree_Partner_2;
    [SerializeField] GameObject btnPartner_2;
    [SerializeField] NFT_Update _NFT_Update_Player;
    [SerializeField] NFT_Update _NFT_Update_Partner_1;
    [SerializeField] NFT_Update _NFT_Update_Partner_2;
    public GameObject lastBtnUse;

    public void OnClickDataCenter()
    {
        dataCenter.SetActive(true);
        OnClickTreeNFT();
        OnClickTreePlayer();
    }
    public void OnClickBack ()
    {
        dataCenter.SetActive(false);
    }
    public void OnClickTreeNFT()
    {
        treeNFT.SetActive(true);
        options.SetActive(false);
        OnClickTreePlayer();
        lastBtnUse = btnTreeNFT.gameObject;
    }
    public void OnClickOptions()
    {
        treeNFT.SetActive(false);
        options.SetActive(true);
        lastBtnUse = btnOptions.gameObject;
    }
    public void OnClickTreePlayer()
    {
        tree_Player.SetActive(true);
        tree_Partner_1.SetActive(false);
        tree_Partner_2.SetActive(false);
        lastBtnUse = btnPlayer.gameObject;
        _NFT_Update_Player.OnClickNFT();
    }

    public void OnClickTreePartner1()
    {
        tree_Player.SetActive(false);
        tree_Partner_1.SetActive(true);
        tree_Partner_2.SetActive(false);
        lastBtnUse = btnPartner_1.gameObject;
        _NFT_Update_Partner_1.OnClickNFT();
    }

    public void OnClickTreePartner2()
    {
        tree_Player.SetActive(false);
        tree_Partner_1.SetActive(false);
        tree_Partner_2.SetActive(true);
        lastBtnUse = btnPartner_2.gameObject;
        _NFT_Update_Partner_2.OnClickNFT();
    }
}
