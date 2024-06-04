using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태
    public event Action onDeath; // 사망시 발동할 이벤트

    protected virtual void OnEnable()
    {
        dead = false; // 사망하지 않은 상태로 시작
        health = startingHealth; // 체력을 시작 체력으로 초기화
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage; // 데미지만큼 체력 감소
        if (health <= 0 && !dead)
        {
            Die(); // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        }
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return; // 이미 사망한 경우 체력을 회복할 수 없음
        }
        health += newHealth; // 체력 추가
    }

    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        onDeath?.Invoke();
        dead = true; // 사망 상태를 참으로 변경
    }

    // 추가: IDamageable 인터페이스의 메서드 구현
    public void OnHit(float damage, Vector3 hitPoint)
    {
        
        health -= damage;
    }
}
