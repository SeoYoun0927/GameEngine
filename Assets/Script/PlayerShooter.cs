using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Animator gunAnimator;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioClip gunshotSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gunAnimator.SetTrigger("Fire");
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        audioSource.PlayOneShot(gunshotSound);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
