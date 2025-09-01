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
    /// 演示两个Box叠加的动画过程，第二个方块跳跃到第一个方块上
    /// </summary>
    public void PlayOnePlusOne(int v, int v1)
    {
        if (_play != null)
        {
            StopCoroutine(_play);
        }


        for (int i = 0; i < alst.Count; i++)
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
       
        _play = play(v, v1);
       
        StartCoroutine(_play);
    }
    IEnumerator _play;
    internal void PlayOneSubOne(int v, int v1)
    {
        if (_play != null)
        {
            StopCoroutine(_play);
        }


        for (int i = 0; i < alst.Count; i++)
        {
            GameObject.Destroy(alst[i]);
        }
        alst.Clear();
        for (int i = 0; i < blst.Count; i++)
        {
            GameObject.Destroy(blst[i]);
        }
        blst.Clear();
        clst.Clear();

        _play = playsub(v, v1);

        StartCoroutine(_play);
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
        //使用dotween把ctrl的distance属性渐变成hei*2
        DOTween.To(() => ctrl.distance, x => ctrl.distance = x,10/*基础高度*/+ hei * 4, 0.5f).SetEase(Ease.OutQuad);
    }

    /**
 * 减法运算
 * */
    IEnumerator playsub(int a, int b)
    {
        //创建被减数
        for (int i = 0; i < a; i++)
        {
            //yield return new WaitForSeconds(1);
            GameObject boxObject1 = GameObject.Instantiate(prefab);
            boxObject1.transform.position = new Vector3(-3, i * 2, 0);

            alst.Add(boxObject1);
            brushctrl();
        }
        //创建减数
        //for (int i = 0; i < b; i++)
        //{
        //    //yield return new WaitForSeconds(1);
        //    GameObject boxObject2 = GameObject.Instantiate(prefab);
        //    boxObject2.transform.position = new Vector3(3, i * 2, 0);
        //    blst.Add(boxObject2);
        //    brushctrl();
        //}
        brushcolor();

        yield return new WaitForSeconds(1);

        //把被减数挪到中间位置 
        for (int i = 0; i < alst.Count; i++)
        {
            Vector3 targetPosition = new Vector3(0, i * 2, 0);
            alst[i].transform.DOMove(targetPosition, 1.0f).SetEase(Ease.OutQuad);
        }
    
        brushcolor();

        yield return new WaitForSeconds(1);

        //把被减数底下的格子变成跟被减数一样的颜色
        //此处假设是将被减数的材质复制给中间位置的格子
        for(int i = 0; i < b;i++)
        {
            clst.Add(alst[0]);
            alst.RemoveAt(0);
        }


        //把减数挪到中间位置（与被减数重叠）
        for (int i = 0; i < clst.Count; i++)
        { 
            Vector3 targetPosition = new Vector3(3, i * 2, 0);
            clst[i].transform.DOMove(targetPosition, 1.0f).SetEase(Ease.OutQuad); 
        }
        yield return new WaitForSeconds(0.2f);
        brushcolor();
        yield return new WaitForSeconds(1);

        //去掉重合的格子（减数部分）
        //for (int i = 0; i < blst.Count; i++)
        //{
        //     GameObject.Destroy(blst[i]); 
        //}
    
        ////清理blst列表中已处理的对象
        //blst.Clear();
        //把clist模型透明处理
        for (int i = 0; i < clst.Count; i++)
        {
            clst[i].GetComponent<MeshRenderer>().material.DOFloat(1, "_Float", 1f).SetEase(Ease.OutQuad);
        }
        yield return new WaitForSeconds(1);

        for (int i = 0; i < clst.Count; i++)
        {
            GameObject.Destroy(clst[i]);
        }
        clst.Clear();

 
        //让剩余的格子落到正确位置
        for (int i = 0; i < alst.Count; i++)
        {
            Vector3 targetPosition = new Vector3(0, i * 2, 0);
            alst[i].transform.DOMove(targetPosition, 1.0f).SetEase(Ease.OutBounce);
        }
    
        brushcolor();
        brushctrl();
    
        yield return new WaitForSeconds(3);
        Main.DispEvent("event_backfromplay");
        _play = null;
    }
     
    IEnumerator play(int a, int b)
    {
        for(int i = 0; i < a; i++)
        {
            yield return new WaitForSeconds(1);
            GameObject boxObject1 = GameObject.Instantiate(prefab);
            boxObject1.transform.position = new Vector3(-3, i*2, 0);
            
            alst.Add(boxObject1);
            brushcolor();
            brushctrl();
        }


        for(int i = 0; i < b; i++)
        {
            yield return new WaitForSeconds(1);
            GameObject boxObject2 = GameObject.Instantiate(prefab);
            boxObject2.transform.position = new Vector3(3, i*2, 0);
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

        yield return new WaitForSeconds(3);
        Main.DispEvent("event_backfromplay");
        _play = null;
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
        // 计算目标位置（第二个方块跳到第一个方块上面）
        Vector3 targetPosition = new Vector3(0, y*2, 0); // 1.5f是因为每个方块高度为1，所以第二个方块底部在1.0f，中心在1.5f

        // 使用DOTween创建跳跃动画
        // 使用.DOJump实现跳跃效果
        box2.DOJump(targetPosition, 1.0f, 1, 1.0f) // 跳跃高度1.0f，1次跳跃，持续1.0秒
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                // 跳跃完成后，改变两个方块的材质
                //box1.GetComponent<Renderer>().material = mat2;
                //box2.GetComponent<Renderer>().material = mat2;
                brushcolor();
                Debug.Log("跳跃动画完成：第二个方块已跳到第一个方块上并改变材质");
            });
         
    }
}
