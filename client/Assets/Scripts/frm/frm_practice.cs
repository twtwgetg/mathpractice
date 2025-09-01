using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class frm_practice : frmbase
{
    public TextMeshProUGUI title, ansure, progress, explan;
    public Button[] buttons;/*索引0到8是数字1到9，索引10是0，索引11是退格键，索引12是确认*/
    private int currentQuestion = 0; // 当前题目索引
    private int totalQuestions = 10; // 总题目数
    private int score = 0; // 总得分
    private string currentAnswer = ""; // 当前答案
    private int correctAnswer = 0; // 正确答案
    private string questionText = ""; // 题目文本
    public Image right, wrong;
    // 在类变量部分添加以下常量和方法
    public const string LAST_PRACTICE_DATE_KEY = "LastPracticeDate";
    public const string CONSECUTIVE_DAYS_KEY = "ConsecutivePracticeDays";


    public Button back, play;
    private bool isProcessing = false; // 是否正在处理答题结果

    private int totalAdditionQuestions = 0; // 加法题目总数
    private int correctAdditionQuestions = 0; // 正确加法题目数
    private int totalSubtractionQuestions = 0; // 减法题目总数
    private int correctSubtractionQuestions = 0; // 正确减法题目数
    private QuestionType currentQuestionType; // 当前题目类型

    // 添加成就相关常量
    public const string ACHIEVEMENT_BEGINNER = "AchievementBeginner";
    public const string ACHIEVEMENT_ADDITION_MASTER = "AchievementAdditionMaster";
    public const string ACHIEVEMENT_SUBTRACTION_MASTER = "AchievementSubtractionMaster";
    public const string ACHIEVEMENT_STREAK_KING = "AchievementStreakKing";
    public const string ACHIEVEMENT_MATH_GENIUS = "AchievementMathGenius";

    private int consecutiveCorrectCount = 0; // 连续正确答题计数
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
            Main.DispEvent("event_play", new int[] { para, parb, correctAnswer });
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
        Main.RegistEvent("gamebegin", (object parm) =>
        {
            hide();
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
        Main.RegistEvent("event_chengjiu", (x) =>
        {
            this.hide();
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
        ResetCurrentStatistics(); // 重置当前练习的统计数据
        GenerateNextQuestion();
    }

    /// <summary>
    /// 生成下一题
    /// </summary>
    void GenerateNextQuestion()
    {
        if (currentQuestion >= totalQuestions)
        {
            SaveStatistics();
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
                currentQuestionType = QuestionType.Addition;
                totalAdditionQuestions++;
                GenerateAdditionQuestion();
                break;
            case QuestionType.Subtraction:
                currentQuestionType = QuestionType.Subtraction;
                totalSubtractionQuestions++;
                GenerateSubtractionQuestion();
                break;
            case QuestionType.Mix:
                if (Random.Range(0, 2) == 0)
                {
                    currentQuestionType = QuestionType.Addition;
                    totalAdditionQuestions++;
                    GenerateAdditionQuestion();
                }
                else
                {
                    currentQuestionType = QuestionType.Subtraction;
                    totalSubtractionQuestions++;    
                    GenerateSubtractionQuestion();
                }
                break;
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
        questionText = $"{para} - {parb}";
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
        if (autopress != null)
        {
            StopCoroutine(autopress);
            autopress = null;
        }


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
            if (currentQuestionType == QuestionType.Addition)
            {
                correctAdditionQuestions++;
            }
            else if (currentQuestionType == QuestionType.Subtraction)
            {
                correctSubtractionQuestions++;
            }
            Debug.Log($"答案正确！当前得分: {score}");
            explan.text = $"正确";
                        // 检查连胜王成就
            CheckStreakKingAchievement();
        }
        else
        {
            explan.text = $"错误,正确答案是: {correctAnswer}";
            Debug.Log($"答案错误！正确答案是: {correctAnswer}");
                        // 重置连续正确计数
            consecutiveCorrectCount = 0;
        }

        progress.text = $"得分{score}/{100}({currentQuestion * 10}%)";

        // 3秒后进入下一题
        DOVirtual.DelayedCall(1.0f, () =>
        {
            explan.text = $"您的答案";
            ansure.text = "_";
            GenerateNextQuestion();
        });
    }
        /// <summary>
    /// 检查连胜王成就（连续正确10道题以上）
    /// </summary>
    void CheckStreakKingAchievement()
    {
        if (consecutiveCorrectCount >= 10 && !PlayerPrefs.HasKey(ACHIEVEMENT_STREAK_KING))
        {
            PlayerPrefs.SetInt(ACHIEVEMENT_STREAK_KING, 1);
            Debug.Log("恭喜获得成就：连胜王！");
            // 这里可以添加成就获得的提示
        }
    }

    void SaveStatistics()
    {
        // 获取已有统计数据
        int savedTotalAddition = PlayerPrefs.GetInt("TotalAdditionQuestions", 0);
        int savedCorrectAddition = PlayerPrefs.GetInt("CorrectAdditionQuestions", 0);
        int savedTotalSubtraction = PlayerPrefs.GetInt("TotalSubtractionQuestions", 0);
        int savedCorrectSubtraction = PlayerPrefs.GetInt("CorrectSubtractionQuestions", 0);
        
        // 更新统计数据
        savedTotalAddition += totalAdditionQuestions;
        savedCorrectAddition += correctAdditionQuestions;
        savedTotalSubtraction += totalSubtractionQuestions;
        savedCorrectSubtraction += correctSubtractionQuestions;
        
        // 保存更新后的数据
        PlayerPrefs.SetInt("TotalAdditionQuestions", savedTotalAddition);
        PlayerPrefs.SetInt("CorrectAdditionQuestions", savedCorrectAddition);
        PlayerPrefs.SetInt("TotalSubtractionQuestions", savedTotalSubtraction);
        PlayerPrefs.SetInt("CorrectSubtractionQuestions", savedCorrectSubtraction);

 // 检查各种成就
        CheckAchievements(savedTotalAddition, savedCorrectAddition, savedTotalSubtraction, savedCorrectSubtraction);
            // 更新连续练习天数
        UpdateConsecutivePracticeDays();
        PlayerPrefs.Save();
        
        Debug.Log($"统计数据已保存 - 加法: {correctAdditionQuestions}/{totalAdditionQuestions}, 减法: {correctSubtractionQuestions}/{totalSubtractionQuestions}");
    }

    /// <summary>
    /// 检查各种成就
    /// </summary>
    void CheckAchievements(int totalAddition, int correctAddition, int totalSubtraction, int correctSubtraction)
    {
        // 检查加法高手成就（加法正确率85%以上）
        if (totalAddition > 0 && !PlayerPrefs.HasKey(ACHIEVEMENT_ADDITION_MASTER))
        {
            float additionAccuracy = (float)correctAddition / totalAddition;
            if (additionAccuracy >= 0.85f)
            {
                PlayerPrefs.SetInt(ACHIEVEMENT_ADDITION_MASTER, 1);
                Debug.Log("恭喜获得成就：加法高手！");
            }
        }

        // 检查减法高手成就（减法正确率80%以上）
        if (totalSubtraction > 0 && !PlayerPrefs.HasKey(ACHIEVEMENT_SUBTRACTION_MASTER))
        {
            float subtractionAccuracy = (float)correctSubtraction / totalSubtraction;
            if (subtractionAccuracy >= 0.8f)
            {
                PlayerPrefs.SetInt(ACHIEVEMENT_SUBTRACTION_MASTER, 1);
                Debug.Log("恭喜获得成就：减法高手！");
            }
        }

        // 检查数学天才成就（总体正确率90%以上）
        int totalAll = totalAddition + totalSubtraction;
        int correctAll = correctAddition + correctSubtraction;
        if (totalAll > 0 && !PlayerPrefs.HasKey(ACHIEVEMENT_MATH_GENIUS))
        {
            float overallAccuracy = (float)correctAll / totalAll;
            if (overallAccuracy >= 0.9f)
            {
                PlayerPrefs.SetInt(ACHIEVEMENT_MATH_GENIUS, 1);
                Debug.Log("恭喜获得成就：数学天才！");
            }
        }
    }

    /// <summary>
    /// 更新连续练习天数
    /// </summary>
    void UpdateConsecutivePracticeDays()
    {
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        string lastPracticeDate = PlayerPrefs.GetString(LAST_PRACTICE_DATE_KEY, "");
        int consecutiveDays = PlayerPrefs.GetInt(CONSECUTIVE_DAYS_KEY, 0);
        
        // 检查是否是同一天练习，避免重复计数
        if (lastPracticeDate == today)
        {
            return;
        }
        
        // 检查是否是连续的一天
        if (!string.IsNullOrEmpty(lastPracticeDate))
        {
            System.DateTime lastDate;
            if (System.DateTime.TryParse(lastPracticeDate, out lastDate))
            {
                System.TimeSpan difference = System.DateTime.Now.Date - lastDate.Date;
                
                // 如果是连续的一天，增加连续天数
                if (difference.Days == 1)
                {
                    consecutiveDays++;
                }
                // 如果间隔超过一天，重置连续天数
                else if (difference.Days > 1)
                {
                    consecutiveDays = 1;
                }
            }
        }
        else
        {
            // 第一次练习，设置为1天
            consecutiveDays = 1;
        }
        
        // 保存更新后的数据
        PlayerPrefs.SetString(LAST_PRACTICE_DATE_KEY, today);
        PlayerPrefs.SetInt(CONSECUTIVE_DAYS_KEY, consecutiveDays);
        
        Debug.Log($"连续练习天数已更新: {consecutiveDays}天");
    }
        /// <summary>
    /// 重置当前练习的统计数据
    /// </summary>
    void ResetCurrentStatistics()
    {
        totalAdditionQuestions = 0;
        correctAdditionQuestions = 0;
        totalSubtractionQuestions = 0;
        correctSubtractionQuestions = 0;
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
        if (currentAnswer.Length > 0)
        {
            ansure.text = $"{currentAnswer}";
        }
        else
        {
            ansure.text = "_";
        }


        if (currentAnswer == correctAnswer.ToString())
        {
            autopress = pressok();
            StartCoroutine(autopress);
        }
    }
    IEnumerator pressok()
    {
        yield return new WaitForSeconds(0.5f);
        OnConfirmButtonPressed();
    }
    IEnumerator autopress = null;

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
          // 检查初学者成就（完成一次测试）
        if (!PlayerPrefs.HasKey(ACHIEVEMENT_BEGINNER))
        {
            PlayerPrefs.SetInt(ACHIEVEMENT_BEGINNER, 1);
            Debug.Log("恭喜获得成就：初学者！");
        }
    }
}