using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int numberofGrenades;
    public int numberofHealth;
    public int numberofEnergy;

    [Header("Stocks")]
    public Text GrenadeStock1;
    public Text GrenadeStock2;
    public Text HealthStock;
    public Text EnergyStock;

    [Header("Health&Energy")]
    public GameObject healthSlot;
    public GameObject energySlot;

    [Header("Ammo & Mag")]
    public Rifle rifle;
    public Bazooka bazooka;
    public Text RifleAmmoText;
    public Text RifleMagText;
    public Text BazookaAmmoText;
    public Text BazookaMagText;

    [Header("Player Things")]
    public GameObject MC;
    public GameObject player;
    public GameObject playerUI;
    public GameObject MMCam;
    public GameObject MMCanvas;
    public GameObject crossHairCanvas;
    public GameObject weaponstopick;
    public GameObject weaponsmenu;

    [Header("Timeline")]
    public GameObject cutScene1;

    private void Update()
    {
        if ((MainMenu.instance!=null && MainMenu.instance.startGame == true))
        {
           // Debug.Log("MainMenu.instance.startGame>>" + MainMenu.instance.startGame);
            MC.SetActive(false);
           // player.SetActive(false);
            playerUI.SetActive(false);
            MMCam.SetActive(false);
            MMCanvas.SetActive(false);
            crossHairCanvas.SetActive(false);
            weaponstopick.SetActive(false);
            weaponsmenu.SetActive(false);

            cutScene1.SetActive(true);

            //Debug.Log("MainMenu ÄÆ¾À ½ÃÀÛ°¡´É"+ MainMenu.instance.startGame);
            if (CutSceneEnder.instance.CutSceneEnd == true)
            {
                MainMenu.instance.startGame = false;
                MC.SetActive(true);
                //player.SetActive(true);
                playerUI.SetActive(true);
                MMCam.SetActive(true);
                MMCanvas.SetActive(true);
                crossHairCanvas.SetActive(true);
                weaponstopick.SetActive(true);
                weaponsmenu.SetActive(true);

                cutScene1.SetActive(false);
            }
        }
        else if((CharacterSelection.instance != null && CharacterSelection.instance.startGame == true))
        {
            //Debug.Log("CharacterSelection.instance.startGame>>" + CharacterSelection.instance.startGame);
            MC.SetActive(false);
            //player.SetActive(false);
            playerUI.SetActive(false);
            MMCam.SetActive(false);
            MMCanvas.SetActive(false);
            crossHairCanvas.SetActive(false);
            weaponstopick.SetActive(false);
            weaponsmenu.SetActive(false);

            cutScene1.SetActive(true);

            //Debug.Log("CharacterSelection ÄÆ¾À ½ÃÀÛ°¡´É"+ CharacterSelection.instance.startGame);
            //Debug.Log("MainMenu ÄÆ¾À ½ÃÀÛ°¡´É"+ MainMenu.instance.startGame);
            if (CutSceneEnder.instance.CutSceneEnd == true)
            {
                CharacterSelection.instance.startGame = false;
                MC.SetActive(true);
                //player.SetActive(true);
                playerUI.SetActive(true);
                MMCam.SetActive(true);
                MMCanvas.SetActive(true);
                //crossHairCanvas.SetActive(true);
                //weaponstopick.SetActive(true);
                //weaponsmenu.SetActive(true);

                cutScene1.SetActive(false);
                CharacterSelection.instance.gameObject.SetActive(false);
            }
        }
       

        //show Ammo & Mag for rifle and bazooka
        RifleAmmoText.text = "" + rifle.presentAmmunition;
        RifleMagText.text = "" + rifle.mag;

        BazookaAmmoText.text = "" + bazooka.presentAmmunition;
        BazookaMagText.text = "" + bazooka.mag;

        //show stock for grenade health and energy
        GrenadeStock1.text = "" + numberofGrenades;
        GrenadeStock2.text = "" + numberofGrenades;
        HealthStock.text = "" + numberofHealth;
        EnergyStock.text = "" + numberofEnergy;

        if(numberofHealth > 0)
        {
            healthSlot.SetActive(true);
        }

        else if(numberofHealth <= 0)
        {
            healthSlot.SetActive(false);
        }


        if (numberofEnergy > 0)
        {
            energySlot.SetActive(true);
        }

        else if (numberofEnergy <= 0)
        {
            energySlot.SetActive(false);
        }
    }
}
