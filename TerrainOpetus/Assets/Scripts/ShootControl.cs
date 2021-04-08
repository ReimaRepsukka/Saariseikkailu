using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootControl : MonoBehaviour
{

    PlayerInputActions pia;

    public Text bulletNumText;
    public GameObject ammoPrefab;

    int bullets = 10;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        pia = new PlayerInputActions();
        pia.Enable();

        RefreshBulletCount();
    }

    // Update is called once per frame
    void Update()
    {
        //Onko painettu shoot-nappia
        if (pia.Land.Shoot.triggered && bullets > 0 && !isShooting)
        {
            isShooting = true;
            //Haetaan ampumiseen sijainti kameran edest‰
            Vector3 shootPos = Camera.main.transform.position
                + Camera.main.transform.forward * 0.5f;

            //Aampumissuunta kameran forward suuntaan
            Vector3 shootDirection = Camera.main.transform.forward;

            //Luodaan ammus valittuun kohtaan.
            GameObject ammo = Instantiate(ammoPrefab, shootPos, Quaternion.identity);

            //Asetetaan ammukselle l‰htˆvoima
            ammo.GetComponent<Rigidbody>().AddForce(shootDirection * 100);

            //Aktivoidaan aseen animaatio
            GetComponent<Animator>().Play("FireGun");
           
            //P‰ivitet‰‰n ammusten m‰‰r‰
            bullets--;
            RefreshBulletCount();
        }
    }

    void RefreshBulletCount()
    {
        if(bulletNumText != null)
            bulletNumText.text = bullets.ToString();
    }

    void MoreAmmo(int ammos)
    {
        bullets += ammos;
        RefreshBulletCount();  
    }

    //T‰t‰ kutsutana animaation lopuksi eventtin‰. M‰‰ritell‰‰n 
    //mallin animaatioiden import-asetuksissa.
    void ShotDone()
    {
        isShooting = false;
    }
}
