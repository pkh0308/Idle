using UnityEngine;

public class DamageText : MonoBehaviour
{
    Animator animator;
    float delay;

    void Awake()
    {
        animator = GetComponent<Animator>();
        delay = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    void OnEnable()
    {
        Invoke(nameof(Disappear), delay);
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}