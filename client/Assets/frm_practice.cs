using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class frm_practice : frmbase
{
    public TextMeshProUGUI title, ansure, progress,explan;
    public Button[] buttons;/*索引0到8是数字1到9，索引10是0，索引11是退格键，索引12是确认*/
    private int currentQuestion = 0; // 当前题目索引
    private int totalQuestions = 10; // 总题目数
    private int score = 0; // 总得分
    private string currentAnswer = ""; // 当前答案
    private int correctAnswer = 0; // 正确答案
    private string questionText = ""; // 题目文本
    public Image right, wrong;

    public Button back,play;
    private bool isProcessing = false; // 是否正在处理答题结果
 
    // 题目类型枚举
    public enum QuestionType
    {
        Addition,    // 加法
        Subtraction, // 减法
      //  Multiplication // 乘法
        Mix,//混合
    }
    QuestionType type_practice = QuestionType.Addition;

    void Awake()
    {
        back.onClick.AddListener(() =>
        {
            Main.DispEvent("event_back");
            hide();
        });
        play.onClick.AddListener(() =>
        {
            Main.DispEvent("event_play",new int[] {para,parb, correctAnswer });
            hide();
        });
        InitializeButtons();

        Main.RegistEvent("event_begin", (x) =>
        {
            BeginPractice();
            return null;
        });
        Main.RegistEvent("event_backfromplay", (x) =>
        {
            show();
            return null;
        });
        
        Main.RegistEvent("event_plus", (x) =>
        {
            
            type_practice = QuestionType.Addition;
            BeginPractice();
            this.show();
            return null;
        });
        Main.RegistEvent("event_subs", (x) =>
        {
            
            type_practice = QuestionType.Subtraction;
            BeginPractice();
            this.show();
            return null;
        });
        Main.RegistEvent("event_mix", (x) =>
        {
            type_practice = QuestionType.Mix;
            BeginPractice();
            this.show();
            return null;
        });
        // 初始化图标状态
        if (right != null) right.gameObject.SetActive(false);
        if (wrong != null) wrong.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化按钮事件
    /// </summary>
    void InitializeButtons()
    {
        // 数字按钮 1-9
        for (int i = 0; i < 9; i++)
        {
            int num = i + 1;
            buttons[i].onClick.AddListener(() => OnNumberButtonPressed(num.ToString()));
        }

        // 数字按钮 0
        buttons[9].onClick.AddListener(() => OnNumberButtonPressed("0"));

        // 退格按钮
        buttons[10].onClick.AddListener(OnBackspaceButtonPressed);

        // 确认按钮
        buttons[11].onClick.AddListener(OnConfirmButtonPressed);
    }

    /// <summary>
    /// 开始练习
    /// </summary>
    void BeginPractice()
    {
        currentQuestion = 0;
        score = 0;
        GenerateNextQuestion();
    }

    /// <summary>
    /// 生成下一题
    /// </summary>
    void GenerateNextQuestion()
    {
        if (currentQuestion >= totalQuestions)
        {
            // 练习结束
            EndPractice();
            return;
        }

        // 隐藏反馈图标
        HideFeedbackIcons();

        // 随机选择题目类型
        QuestionType type = type_practice;// (QuestionType)Random.Range(0, 2);

        // 根据题目类型生成题目
        switch (type)
        {
            case QuestionType.Addition:
                GenerateAdditionQuestion();
                break;
            case QuestionType.Subtraction:
                GenerateSubtractionQuestion();
                break;
            case QuestionType.Mix:
                if (Random.Range(0, 2) == 0)
                {
                    GenerateAdditionQuestion();
                }
                else
                {
                    GenerateSubtractionQuestion();
                }
                break;
            //case QuestionType.Multiplication:
            //    GenerateMultiplicationQuestion();
            //    break;
        }

        // 更新题目显示
        title.text = $"第{currentQuestion + 1}题: {questionText} = ?";
        currentAnswer = "";
        isProcessing = false;
        currentQuestion++;
    }
    int para, parb;
    /// <summary>
    /// 生成加法题目
    /// </summary>
    void GenerateAdditionQuestion()
    {
        para = Random.Range(1, 5); // 1-20之间的数
        parb = Random.Range(1, 5);
        correctAnswer = para + parb;
        questionText = $"{para} + {parb}";
    }

    /// <summary>
    /// 生成减法题目
    /// </summary>
    void GenerateSubtractionQuestion()
    {
        para = Random.Range(1, 10); // 1-30之间的数
        parb = Random.Range(1, para);  // 确保结果为正数
        correctAnswer = para - parb;
        questionText = $"{para} - {para}";
    }

    /// <summary>
    /// 生成乘法题目
    /// </summary>
    void GenerateMultiplicationQuestion()
    {
        int a = Random.Range(1, 13); // 1-12之间的数
        int b = Random.Range(1, 13);
        correctAnswer = a * b;
        questionText = $"{a} × {b}";
    }

    /// <summary>
    /// 数字按钮按下事件
    /// </summary>
    /// <param name="number">按下的数字</param>
    void OnNumberButtonPressed(string number)
    {
        if (isProcessing) return; // 如果正在处理结果，则忽略输入

        // 限制答案长度（最多3位数）
        if (currentAnswer.Length < 3)
        {
            currentAnswer += number;
            UpdateTitleDisplay();
        }
    }

    /// <summary>
    /// 退格按钮按下事件
    /// </summary>
    void OnBackspaceButtonPressed()
    {
        if (isProcessing) return; // 如果正在处理结果，则忽略输入

        if (currentAnswer.Length > 0)
        {
            currentAnswer = currentAnswer.Substring(0, currentAnswer.Length - 1);
            UpdateTitleDisplay();
        }
    }

    /// <summary>
    /// 确认按钮按下事件
    /// </summary>
    void OnConfirmButtonPressed()
    {
        if (isProcessing || string.IsNullOrEmpty(currentAnswer)) return;

        isProcessing = true;
        int answer = int.Parse(currentAnswer);

        // 判断答案是否正确
        if (answer == correctAnswer)
        {
            ShowFeedback(true);
        }
        else
        {
            ShowFeedback(false);
        }
    }

    /// <summary>
    /// 显示答题反馈
    /// </summary>
    /// <param name="isCorrect">答案是否正确</param>
    void ShowFeedback(bool isCorrect)
    {
        // 显示对应的图标
        if (isCorrect)
        {
            if (right != null)
            {
                right.gameObject.SetActive(true);
                // 添加动画效果
                right.transform.localScale = Vector3.zero;
                right.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            if (wrong != null)
            {
                wrong.gameObject.SetActive(true);
                // 添加动画效果
                wrong.transform.localScale = Vector3.zero;
                wrong.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }

        // 更新得分
        if (isCorrect)
        {
            score += 10; // 每题10分
            Debug.Log($"答案正确！当前得分: {score}");
            explan.text = $"正确";
        }
        else
        {
            explan.text = $"错误,正确答案是: {correctAnswer}";
            Debug.Log($"答案错误！正确答案是: {correctAnswer}");
        }

        progress.text = $"得分{score}/{100}({currentQuestion * 10}%)";
        
        // 3秒后进入下一题
        DOVirtual.DelayedCall(1.0f, () => {
            explan.text = $"您的答案";
            ansure.text = "_";
            GenerateNextQuestion();
        });
    }

    /// <summary>
    /// 隐藏反馈图标
    /// </summary>
    void HideFeedbackIcons()
    {
        if (right != null) right.gameObject.SetActive(false);
        if (wrong != null) wrong.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新题目显示
    /// </summary>
    void UpdateTitleDisplay()
    {
        //title.text = $"第{currentQuestion}题: {questionText} = {currentAnswer}";
        ansure.text = $"{currentAnswer}";
    }
    
    /// <summary>
    /// 结束练习
    /// </summary>
    void EndPractice()
    {
        HideFeedbackIcons();
        progress.text = $"练习结束！总得分: {score}/100";
        Debug.Log($"练习结束！总得分: {score}/100");
        Main.DispEvent("event_over", new object[] { score, 30 });
        // 可以在这里添加其他结束逻辑，比如显示评价等
    }
}