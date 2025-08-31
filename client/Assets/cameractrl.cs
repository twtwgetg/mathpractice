using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameractrl : MonoBehaviour
{
    public float distance = 3;

    private float currentX = 0.0f; // ��ǰˮƽ��ת�Ƕ�
    private float currentY = 0.0f; // ��ǰ��ֱ��ת�Ƕ�
    // Start is called before the first frame update
    void Start()
    {
        // ��ʼ����ת�Ƕ�
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // �������Ŀ��λ��
        Vector3 targetPosition = new Vector3(0, 0, 0);// target.position + Vector3.up * height;
        Vector3 cameraPosition = targetPosition + rotation * direction;

        // �������λ�úͳ���
        transform.position = cameraPosition;
        transform.LookAt(targetPosition);
    }
}
