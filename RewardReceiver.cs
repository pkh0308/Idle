using UnityEngine;

// 메인 스레드에서 광고 보상을 수령하기 위한 클래스
public class RewardReceiver : MonoBehaviour
{
    void Update()
    {
        if(Managers.Ad.AdPaid == false)
            return;

        Managers.Ad.GetReward();
    }
}