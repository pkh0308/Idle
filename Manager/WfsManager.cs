using System.Collections.Generic;
using UnityEngine;

// WaitForSeconds ������ Ŭ����
// �ش� Ŭ������ ���� �̹� ������ WaitForSeconds ��ü ��ȯ
// ���� �������� �ʴ� �ð��� WaitForSeconds�� ��ü ���� ����
public class WfsManager
{
    Dictionary<float, WaitForSeconds> secondsDic = new Dictionary<float, WaitForSeconds>();

    public void Init() { }

    public WaitForSeconds GetWaitForSeconds(float time)
    {
        // �ش� �ð��� WaitForSeconds�� ���� ���
        if (secondsDic.TryGetValue(time, out WaitForSeconds value))
            return value;

        // ���� ���
        secondsDic.Add(time, new WaitForSeconds(time));
        return secondsDic[time];
    }
}