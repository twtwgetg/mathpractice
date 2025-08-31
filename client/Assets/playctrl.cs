using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using DG.Tweening;
using System;

public class playctrl : MonoBehaviour
{
    public Material mat1;
    public Material mat2,mat3,mat4,mat5,mat6,mat7,mat8,mat9,mat10;  
    public GameObject prefab;
    private void Start()
    {
        
    }
 
    /// <summary>
    /// ��ʾ����Box���ӵĶ������̣��ڶ���������Ծ����һ��������
    /// </summary>
    public void PlayOnePlusOne(int v, int v1)
    {
        for(int i = 0; i < alst.Count; i++)
        {
            GameObject.Destroy(alst[i]);
        }
        alst.Clear();
        for(int i = 0; i < blst.Count; i++)
        {
            GameObject.Destroy(blst[i]);
        }
        blst.Clear();
        clst.Clear(); 
        StartCoroutine(play(v, v1));
    }

    internal void PlayOneSubOne(int v1, int v2)
    {
        throw new NotImplementedException();
    }

    cameractrl _ctrl;

    cameractrl ctrl
    {
        get
        {
            if (_ctrl == null)
            {
                _ctrl = Camera.main.gameObject.GetComponent<cameractrl>();//.GetComponent<cameractrl>;
            }
            return _ctrl;
        }
    }
    List<GameObject> alst = new List<GameObject>();
    List<GameObject> blst = new List<GameObject>();
    List<GameObject> clst = new List<GameObject>();
    void brushctrl()
    {
        int hei = 0;
        hei = Mathf.Max(hei, alst.Count);
        hei = Mathf.Max(hei, blst.Count);
        hei = Mathf.Max(hei, clst.Count);
        //ʹ��dotween��ctrl��distance���Խ����hei*2
        DOTween.To(() => ctrl.distance, x => ctrl.distance = x,10/*�����߶�*/+ hei * 4, 0.5f).SetEase(Ease.OutQuad);
    }
    IEnumerator play(int a, int b)
    {
        for(int i = 0; i < a; i++)
        {
            yield return new WaitForSeconds(1);
            GameObject boxObject1 = GameObject.Instantiate(prefab);
            boxObject1.transform.position = new Vector3(-2, i*2, 0);
            
            alst.Add(boxObject1);
            brushcolor();
            brushctrl();
        }


        for(int i = 0; i < b; i++)
        {
            yield return new WaitForSeconds(1);
            GameObject boxObject2 = GameObject.Instantiate(prefab);
            boxObject2.transform.position = new Vector3(2, i*2, 0);
            blst.Add(boxObject2);
            brushcolor();
            brushctrl();
        }

        yield return new WaitForSeconds(1);

        for(int i = 0; i < alst.Count; i++)
        {
            AnimateBoxes(i, alst[i].transform); 
        }
        for(int i = 0; i < blst.Count; i++)
        {
            AnimateBoxes(i+alst.Count, blst[i].transform);
        }
        brushctrl(); 
    }
    Material getmat(int count,int index=1)
    {
        if (count == 1)
        {
            return mat1;
        }
        else if (count == 2)
        {
            return mat2;
        }
        else if (count == 3)
        {
            return mat3;
        }
        else if (count == 4)
        {
            return mat4;
        }
        else if (count == 5)
        {
            return mat5;
        }
        else if (count == 6)
        {
            return mat6;
        }
        else if(count == 7)
        {
            if (index == 0) return mat1;
            if (index == 1) return mat2;
            if (index == 2) return mat3;
            if (index == 3) return mat4;
            if (index == 4) return mat5;
            if (index == 5) return mat6;
            return mat7;
        }
        else if (count == 8)
        {
            return mat8;
        }
        else if (count == 9)
        {
            if (!mat9list.ContainsKey(index))
            {
                mat9list[index] = Instantiate(mat9);
                mat9list[index].color = Color.Lerp(new Color(0.9f, 0.9f, 0.9f), new Color(0.5f, 0.5f, 0.5f), index / 8f);
            }
            return mat9list[index];
        }
        else
        {
            return mat10;
        }
    }
    Dictionary<int, Material> mat9list = new Dictionary<int, Material>();
    void brushcolor()
    {
        for(int i = 0; i < alst.Count; i++)
        {
            alst[i].GetComponent<MeshRenderer>().material = getmat(alst.Count,i); 
        }
        for (int i = 0; i < blst.Count; i++)
        {
            blst[i].GetComponent<MeshRenderer>().material = getmat(blst.Count,i);
        }

        for (int i = 0; i < clst.Count; i++)
        {
            clst[i].GetComponent<MeshRenderer>().material = getmat(clst.Count,i);
        }
    }
    void AnimateBoxes(int y,Transform box2 )
    {
        clst.Add(box2.gameObject);
        // ����Ŀ��λ�ã��ڶ�������������һ���������棩
        Vector3 targetPosition = new Vector3(0, y*2, 0); // 1.5f����Ϊÿ������߶�Ϊ1�����Եڶ�������ײ���1.0f��������1.5f

        // ʹ��DOTween������Ծ����
        // ʹ��.DOJumpʵ����ԾЧ��
        box2.DOJump(targetPosition, 1.0f, 1, 1.0f) // ��Ծ�߶�1.0f��1����Ծ������1.0��
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                // ��Ծ��ɺ󣬸ı���������Ĳ���
                //box1.GetComponent<Renderer>().material = mat2;
                //box2.GetComponent<Renderer>().material = mat2;
                brushcolor();
                Debug.Log("��Ծ������ɣ��ڶ���������������һ�������ϲ��ı����");
            });

        // ͬʱ���Ը���һ������һ����΢�����ŷ���Ч��
        //box1.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 10, 1)
        //    .SetDelay(0.5f); // ����Ծ�����и��跴��
    }
}
