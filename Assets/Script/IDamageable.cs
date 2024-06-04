// �������̽��� ����ϴ� ������ ���� ��ü�� ���������� ������ �� �ִ� ����� �����ϱ� �����̴�.
// ���� ��� �پ��� ������ ĳ���Ͱ� IDamageable �������̽��� ��ӹ޾� OnDamage(), OnHit() �Լ��� �����Ѵٸ�,
// �ش� �Լ����� ������ ĳ���Ϳ� �°� �����ϰ� �ȴ�.

using UnityEngine;


public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
    void OnHit(float Damage, Vector3 hitNormal);
}
