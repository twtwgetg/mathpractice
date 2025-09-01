using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameractrl : MonoBehaviour
{
    public float distance = 3;

    private float currentX = 0.0f; // 当前水平旋转角度
    private float currentY = 0.0f; // 当前垂直旋转角度
    // Start is called before the first frame update
    void Start()
    {
        // 初始化旋转角度
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 计算相机目标位置
        Vector3 targetPosition = new Vector3(0, 0, 0);// target.position + Vector3.up * height;
        Vector3 cameraPosition = targetPosition + rotation * direction;

        // 设置相机位置和朝向
        transform.position = cameraPosition;
        transform.LookAt(targetPosition);
    }
}
