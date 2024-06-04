// 인터페이스를 사용하는 이유는 여러 객체에 공통적으로 적용할 수 있는 기능을 정의하기 위함이다.
// 예를 들어 다양한 종류의 캐릭터가 IDamageable 인터페이스를 상속받아 OnDamage(), OnHit() 함수를 구현한다면,
// 해당 함수들은 각각의 캐릭터에 맞게 동작하게 된다.

using UnityEngine;


public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
    void OnHit(float Damage, Vector3 hitNormal);
}
