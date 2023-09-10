using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class EnemyUnit : MonoBehaviour
{
    public int health = 3;
    public int Damage = 1;
    public float AttackRadius = 15f;
    public float NoticeRadius = 30f;
    public Transform GunEnd;

    private bool ReadyToShoot = true;
    private LineRenderer lineRenderer;
    private Transform Player;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        float distance = Vector3.Distance(Player.position, this.transform.position);

        if (distance < AttackRadius && ReadyToShoot)
        {
            this.transform.LookAt(Player);
            //this.transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z - 90);
            StartCoroutine(Shoot());
        }
        if (distance < NoticeRadius && distance > AttackRadius)
        {
            agent.SetDestination(Player.position);
        }
        if (distance <= agent.stoppingDistance)
        {
            this.transform.LookAt(Player);
        }
    }

    IEnumerator Shoot()
    {
        ReadyToShoot = false;
        float Time_Difference = Random.Range(0.2f, 1);
        Vector3 targetForShot = Player.position;
        yield return new WaitForSeconds(Time_Difference);
        GameObject Gun = GunEnd.parent.gameObject;
        Ray ray = new Ray(GunEnd.position, targetForShot - GunEnd.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject== Player.gameObject)
            {
                Player.GetComponent<Player_Controller>().health -= Gun.GetComponent<Gun_Shoot>().GunStats.Damage_Per_Shot;
            }
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, GunEnd.position);
        lineRenderer.SetPosition(1, targetForShot);

        yield return new WaitForSeconds(Gun.GetComponent<Gun_Shoot>().ShotEffectDuration);
        lineRenderer.enabled = false;
        ReadyToShoot = false;
        StartCoroutine(WaitBetweenShots());
    }

    IEnumerator WaitBetweenShots()
    {
        GameObject Gun = GunEnd.parent.gameObject;
        yield return new WaitForSeconds(Gun.GetComponent<Gun_Shoot>().GunStats.Time_Between_Shots);
        ReadyToShoot = true;
    }
}
