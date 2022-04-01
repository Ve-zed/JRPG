using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_DataCenter : MonoBehaviour
{
    [SerializeField] GameObject dataCenter;
    [SerializeField] GameObject treeNFT;
    [SerializeField] GameObject options;
    [SerializeField] GameObject tree_Player;
    [SerializeField] GameObject tree_Partner_1;
    [SerializeField] GameObject tree_Partner_2;
    [SerializeField] GameObject commandes;
    [SerializeField] GameObject son;
    [SerializeField] NFT_Update _NFT_Update_Player;
    [SerializeField] NFT_Update _NFT_Update_Partner_1;
    [SerializeField] NFT_Update _NFT_Update_Partner_2;
    [SerializeField] Btn_MenuProperties _btnUpdate1;
    [SerializeField] Btn_MenuProperties _btnUpdate2;
    private Monster _Hero;

    public void OnClickDataCenter()
    {
        dataCenter.SetActive(true);
        _btnUpdate1.OnClickBtn();
        _btnUpdate2.OnClickBtn();
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
        _btnUpdate1.OnClickBtn();
        OnClickTreePlayer();
    }
    public void OnClickOptions()
    {
        treeNFT.SetActive(false);
        options.SetActive(true);
        _btnUpdate1.OnClickBtn();
    }
    public void OnClickTreePlayer()
    {
        tree_Player.SetActive(true);
        tree_Partner_1.SetActive(false);
        tree_Partner_2.SetActive(false);
        _NFT_Update_Player.OnClickNFT();
        //_Hero = ;
    }
    public void OnClickTreePartner1()
    {
        tree_Player.SetActive(false);
        tree_Partner_1.SetActive(true);
        tree_Partner_2.SetActive(false);
        _NFT_Update_Partner_1.OnClickNFT();
    }
    public void OnClickTreePartner2()
    {
        tree_Player.SetActive(false);
        tree_Partner_1.SetActive(false);
        tree_Partner_2.SetActive(true);
        _NFT_Update_Partner_2.OnClickNFT();
    }
    public void OnClickCommande()
    {
        commandes.SetActive(true);
        son.SetActive(false);
    }
    public void OnClickSon()
    {
        commandes.SetActive(false);
        son.SetActive(true);
    }
}
