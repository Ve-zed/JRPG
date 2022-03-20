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
    }
    public void OnClickOptions()
    {
        treeNFT.SetActive(false);
        options.SetActive(true);
    }

    public void OnClickTreePlayer()
    {
        tree_Player.SetActive(true);
        tree_Partner_1.SetActive(false);
        tree_Partner_2.SetActive(false);
    }
    public void OnClickTreePartner1()
    {
        tree_Player.SetActive(false);
        tree_Partner_1.SetActive(true);
        tree_Partner_2.SetActive(false);
    }
    public void OnClickTreePartner2()
    {
        tree_Player.SetActive(false);
        tree_Partner_1.SetActive(false);
        tree_Partner_2.SetActive(true);
    }
}
