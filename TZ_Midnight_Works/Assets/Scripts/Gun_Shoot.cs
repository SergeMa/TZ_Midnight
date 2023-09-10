using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Gun_Shoot : MonoBehaviour
{
    private int Damage;
    public bool ReadyToShoot = true;
    private LineRenderer lineRenderer;
    private Transform CameraTransform;
    private Camera Camera;

    public int CurrentWeaponIndex = 1;
    public int CurrentAmmo = 1;
    public float ShotEffectDuration = 0.5f;
    public Transform GunEnd;
    public Gun_Scriptable GunStats;
    public TextMeshProUGUI AmmoCountShow;


    // Start is called before the first frame update
    void Start()
    {
        CameraTransform = GameObject.FindGameObjectWithTag("Camera").transform;
        Camera = CameraTransform.GetComponent<Camera>();
        CurrentAmmo = GunStats.Bullets_In_Magazine;
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentAmmo > 0)
        {
            AmmoCountShow.text = "Ammo: " + CurrentAmmo + "/" + GunStats.Bullets_In_Magazine;
        }
        else
        {
            AmmoCountShow.text = "Ammo: Reloading/" + GunStats.Bullets_In_Magazine;
        }

        if (Input.GetMouseButtonDown(0) && CurrentAmmo > 0 && ReadyToShoot)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapWeapon();
        }
    }

    private void OnEnable()
    {
        if(CurrentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void SwapWeapon()
    {
        CurrentWeaponIndex = (CurrentWeaponIndex + 1) % 3;
        GameObject nextWeapon = transform.parent.GetChild(CurrentWeaponIndex).gameObject;
        nextWeapon.SetActive(true);
        nextWeapon.GetComponent<Gun_Shoot>().CurrentWeaponIndex = CurrentWeaponIndex;

        gameObject.SetActive(false);
    }

    private void Shoot()
    {
        CurrentAmmo -= 1;
        Ray CameraOutput = Camera.ViewportPointToRay(new Vector3(0.5f,0.5f, 0));
        Vector3 Shoot_Result;
        RaycastHit hit;
        if (Physics.Raycast(CameraOutput, out hit))
        {
            Shoot_Result = hit.point;
            if(hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyUnit>().health -= GunStats.Damage_Per_Shot;
            }
        }
        else
        {
            Shoot_Result = CameraTransform.position + CameraTransform.forward * 100;
        }
        lineRenderer.SetPosition(0, GunEnd.position);
        lineRenderer.SetPosition(1, Shoot_Result);
        StartCoroutine(ShotEffect());
        ReadyToShoot = !ReadyToShoot;
        if (CurrentAmmo > 0)
        {
            StartCoroutine(Wait_Time_Betwen_Shots());
        }
        else
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Wait_Time_Betwen_Shots()
    {
        yield return new WaitForSeconds(GunStats.Time_Between_Shots);
        ReadyToShoot = true;
    }

    private IEnumerator Reload()
    {
        GetComponent<Animator>().SetTrigger("Reloading");
        ReadyToShoot = false;
        CurrentAmmo = 0;
        yield return new WaitForSeconds(GunStats.Reload_Time);
        CurrentAmmo = GunStats.Bullets_In_Magazine;
        ReadyToShoot = true;
    }

    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(ShotEffectDuration);
        lineRenderer.enabled = false;
    }
}
