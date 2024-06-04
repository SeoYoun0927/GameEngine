using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f; // ���� ü��
    public float health { get; protected set; } // ���� ü��
    public bool dead { get; protected set; } // ��� ����
    public event Action onDeath; // ����� �ߵ��� �̺�Ʈ

    protected virtual void OnEnable()
    {
        dead = false; // ������� ���� ���·� ����
        health = startingHealth; // ü���� ���� ü������ �ʱ�ȭ
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage; // ��������ŭ ü�� ����
        if (health <= 0 && !dead)
        {
            Die(); // ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        }
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return; // �̹� ����� ��� ü���� ȸ���� �� ����
        }
        health += newHealth; // ü�� �߰�
    }

    public virtual void Die()
    {
        // onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        onDeath?.Invoke();
        dead = true; // ��� ���¸� ������ ����
    }

    // �߰�: IDamageable �������̽��� �޼��� ����
    public void OnHit(float damage, Vector3 hitPoint)
    {
        
        health -= damage;
    }
}
