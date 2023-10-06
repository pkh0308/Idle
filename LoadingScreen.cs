using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    TextMeshProUGUI[] texts;

    float _minY;
    float _maxY;
    Vector2 _offset;

    const float Limit = 100.0f;

    void Start()
    {
        texts = transform.GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();
        _minY = texts[0].rectTransform.anchoredPosition.y;
        _maxY = _minY + Limit;

        _offset = new Vector2(0, 8);
    }

    int curIdx = 0;
    bool toUpper = true;
    void Update()
    {
        if(toUpper) 
        {
            texts[curIdx].rectTransform.anchoredPosition += _offset;
            // �ְ� ���̺��� �������� ���� ��ȯ
            if (texts[curIdx].rectTransform.anchoredPosition.y > _maxY)
            {
                toUpper = false;
            }
        }
        else
        {
            texts[curIdx].rectTransform.anchoredPosition -= _offset;
            // ���� ���̺��� �������� ���� �ؽ�Ʈ�� �̵�
            if (texts[curIdx].rectTransform.anchoredPosition.y < _minY)
            {
                toUpper = true;
                curIdx = (curIdx + 1) % texts.Length;
            }
        }
    }
}