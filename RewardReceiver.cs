using UnityEngine;

// ���� �����忡�� ���� ������ �����ϱ� ���� Ŭ����
public class RewardReceiver : MonoBehaviour
{
    void Update()
    {
        if(Managers.Ad.AdPaid == false)
            return;

        Managers.Ad.GetReward();
    }
}