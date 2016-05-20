using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public Sprite[] gusLives;
    public Sprite[] lydiaLives;
    public Image[] vineCount;
    public Image[] spireCount;
    public Image GusLivesUI;
    public Image LydiaLivesUI;
    public Slider GusElementalSlider;
    public Slider GusChargeSlider;
    public Slider GusRechargeSlider;
    public Slider LydiaElementalSlider;
    public Slider LydiaChargeSlider;
    public Slider LydiaRechargeSlider;
    public GameObject GameOverOverlay;
    public Text winnerLabel;
    public Sprite GusWins;
    public Sprite LydiaWins;
    public Image GameOverImage;

    private GameObject Gus;
    private GameObject Lydia;
    private PlayerController GusControl;
    private PlayerController LydiaControl;
    private SpireAbility spireAbility;
    private VineAbility vineAbility;
    private ProjectileAbility GusProjectile;
    private ProjectileAbility LydiaProjectile;

    private float defaultChargeMeterY;
    private bool gameOver = false;
	// Use this for initialization
	void Start () {
        Gus = GameObject.FindGameObjectWithTag("Gus");
        Lydia = GameObject.FindGameObjectWithTag("Lydia");

        GusControl = Gus.GetComponent<PlayerController>();
        LydiaControl = Lydia.GetComponent<PlayerController>();

        spireAbility = Gus.GetComponent<SpireAbility>();
        vineAbility = Lydia.GetComponent<VineAbility>();

        GusProjectile = Gus.GetComponent<ProjectileAbility>();
        LydiaProjectile = Lydia.GetComponent<ProjectileAbility>();

        defaultChargeMeterY = LydiaChargeSlider.GetComponent<RectTransform>().position.y;
    }
	
	// Update is called once per frame
	void Update () {
        //display player lives
        GusLivesUI.sprite = gusLives[GusControl.GetLives()];
        LydiaLivesUI.sprite = lydiaLives[LydiaControl.GetLives()];
        GusElementalSlider.value = GusProjectile.GetCount();
        LydiaElementalSlider.value = LydiaProjectile.GetCount();

        //Fill up charge meter based on cooldown time of projectile ability for Gus
        RectTransform rt = GusChargeSlider.GetComponent<RectTransform>();
        Vector3 newPos = rt.position;
        newPos.y = defaultChargeMeterY - (5 - GusElementalSlider.value) * 39;
        rt.position = newPos;
        GusChargeSlider.value = GusProjectile.getChargedProjectiles();

        if (GusProjectile.GetCount() < GusProjectile.maxProjectileCount)
            GusRechargeSlider.value = GusProjectile.getRechargeValue();
        else
            GusRechargeSlider.value = 0;

        //Fill up charge meter based on cooldown time of projectile ability for Lydia
        rt = LydiaChargeSlider.GetComponent<RectTransform>();
        newPos = rt.position;
        newPos.y = defaultChargeMeterY - (5 - LydiaElementalSlider.value) * 39;
        rt.position = newPos;
        LydiaChargeSlider.value = LydiaProjectile.getChargedProjectiles();

        if (LydiaProjectile.GetCount() < LydiaProjectile.maxProjectileCount)
            LydiaRechargeSlider.value = LydiaProjectile.getRechargeValue();
        else
            LydiaRechargeSlider.value = 0;


        for(int i = 0; i < 3; i++)
        {
            if(i < vineAbility.getVineCount())
            {
                vineCount[i].enabled = false;
            }
            else
            {
                vineCount[i].enabled = true;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (i < spireAbility.getSpireCount())
            {
                spireCount[i].enabled = false;
            }
            else
            {
                spireCount[i].enabled = true;
            }
        }
    }

    public void gameOverHudOn()
    {
        GameOverOverlay.SetActive(true);
        int LydiaMarkerCount = GameObject.FindGameObjectsWithTag("LydiaMarker").Length;
        int GusMarkerCount = GameObject.FindGameObjectsWithTag("GusMarker").Length;
        if (LydiaMarkerCount > GusMarkerCount)
        {
            GameOverImage.sprite = LydiaWins;
        }
        else if(LydiaMarkerCount < GusMarkerCount)
        {
            GameOverImage.sprite = GusWins;
        }
        else
        {
            GameOverImage.sprite = GusWins;
        }
        winnerLabel.text += "Fire Markers: " + GusMarkerCount + "\nFlower Markers: " + LydiaMarkerCount;
        gameOver = true;
    }
}
