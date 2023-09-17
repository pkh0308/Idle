using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    TextMeshProUGUI dmgText;
    Animator animator;
    float delay;

    void Awake()
    {
        dmgText = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        delay = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    void OnEnable()
    {
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        yield return Managers.Wfs.GetWaitForSeconds(delay - 0.5f);

        Color curColor = dmgText.color;
        while (dmgText.color.a > 0)
        {
            curColor.a -= 0.1f;
            dmgText.color = curColor;
            yield return Managers.Wfs.GetWaitForSeconds(0.05f);
        }

        gameObject.SetActive(false);
    }
}