// 좀비 AI 구현
using System;
using UnityEngine;
using UnityEngine.AI;


public class Zombie : LivingEntity
{
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private LivingEntity targetEntity; // 추적 대상
    private UnityEngine.AI.NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
    public AudioClip deathSound; // 사망 시 재생할 소리
    public AudioClip hitSound; // 피격 시 재생할 소리

    private Animator zombieAnimator; // 애니메이터 컴포넌트
    private AudioSource zombieAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer zombieRenderer; // 렌더러 컴포넌트

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    public bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }
    private void Awake()
    {
        // 초기화
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();
        zombieRenderer = GetComponInChildren<Renderer>();
    }

    public void Setup(ZombieData zombieData)
    {
        // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
        startingHealth = zombieData.health;
        health = zombieData.health;
        damage = zombieData.damage;
        navMeshAgent.speed = zombieData.speed;
        zombieRenderer.material.color = zombieData.skinColor;
    }
    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }
    private void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }
    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        // 살아 있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                navMeshAgent.isStopped = true;
                // 주변 20f 범위 내에 있는 모든 콜라이더를 가져온다.
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 콜라이더에 연결된 LivingEntity 컴포넌트를 가져온다.
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    // LivingEntity가 null이 아니고 죽지 않았다면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // 추적 대상을 현재 LivingEntity로 설정한다.
                        targetEntity = livingEntity;
                        // 루프를 탈출한다.
                        break;
                    }
                }
            }
            // 0.25초 주기로 처리를 반복한다.
            yield return new WaitForSeconds(0.25f);
        }
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
            zombieAudioPlayer.PlayOneShot(hitSound);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();
        Collider[] zombieColliders = GetComponent<Collider>();
        for (int i = 0; i < zombieColliders.Length; i++)
        {
        }
        zombieColliders[i].enabled = false;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        zombieAnimator.SetTrigger("Die");
        zombieAudioPlayer.PlayOneShot(deathSound);
    }
    private void OnTriggerStay(Collider other)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}