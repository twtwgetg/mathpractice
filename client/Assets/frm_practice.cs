using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class frm_practice : frmbase
{
    public TextMeshProUGUI title, ansure, progress,explan;
    public Button[] buttons;/*����0��8������1��9������10��0������11���˸��������12��ȷ��*/
    private int currentQuestion = 0; // ��ǰ��Ŀ����
    private int totalQuestions = 10; // ����Ŀ��
    private int score = 0; // �ܵ÷�
    private string currentAnswer = ""; // ��ǰ��
    private int correctAnswer = 0; // ��ȷ��
    private string questionText = ""; // ��Ŀ�ı�
    public Image right, wrong;

    public Button back,play;
    private bool isProcessing = false; // �Ƿ����ڴ��������
 
    // ��Ŀ����ö��
    public enum QuestionType
    {
        Addition,    // �ӷ�
        Subtraction, // ����
      //  Multiplication // �˷�
        Mix,//���
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
        // ��ʼ��ͼ��״̬
        if (right != null) right.gameObject.SetActive(false);
        if (wrong != null) wrong.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��ʼ����ť�¼�
    /// </summary>
    void InitializeButtons()
    {
        // ���ְ�ť 1-9
        for (int i = 0; i < 9; i++)
        {
            int num = i + 1;
            buttons[i].onClick.AddListener(() => OnNumberButtonPressed(num.ToString()));
        }

        // ���ְ�ť 0
        buttons[9].onClick.AddListener(() => OnNumberButtonPressed("0"));

        // �˸�ť
        buttons[10].onClick.AddListener(OnBackspaceButtonPressed);

        // ȷ�ϰ�ť
        buttons[11].onClick.AddListener(OnConfirmButtonPressed);
    }

    /// <summary>
    /// ��ʼ��ϰ
    /// </summary>
    void BeginPractice()
    {
        currentQuestion = 0;
        score = 0;
        GenerateNextQuestion();
    }

    /// <summary>
    /// ������һ��
    /// </summary>
    void GenerateNextQuestion()
    {
        if (currentQuestion >= totalQuestions)
        {
            // ��ϰ����
            EndPractice();
            return;
        }

        // ���ط���ͼ��
        HideFeedbackIcons();

        // ���ѡ����Ŀ����
        QuestionType type = type_practice;// (QuestionType)Random.Range(0, 2);

        // ������Ŀ����������Ŀ
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

        // ������Ŀ��ʾ
        title.text = $"��{currentQuestion + 1}��: {questionText} = ?";
        currentAnswer = "";
        isProcessing = false;
        currentQuestion++;
    }
    int para, parb;
    /// <summary>
    /// ���ɼӷ���Ŀ
    /// </summary>
    void GenerateAdditionQuestion()
    {
        para = Random.Range(1, 5); // 1-20֮�����
        parb = Random.Range(1, 5);
        correctAnswer = para + parb;
        questionText = $"{para} + {parb}";
    }

    /// <summary>
    /// ���ɼ�����Ŀ
    /// </summary>
    void GenerateSubtractionQuestion()
    {
        para = Random.Range(1, 10); // 1-30֮�����
        parb = Random.Range(1, para);  // ȷ�����Ϊ����
        correctAnswer = para - parb;
        questionText = $"{para} - {para}";
    }

    /// <summary>
    /// ���ɳ˷���Ŀ
    /// </summary>
    void GenerateMultiplicationQuestion()
    {
        int a = Random.Range(1, 13); // 1-12֮�����
        int b = Random.Range(1, 13);
        correctAnswer = a * b;
        questionText = $"{a} �� {b}";
    }

    /// <summary>
    /// ���ְ�ť�����¼�
    /// </summary>
    /// <param name="number">���µ�����</param>
    void OnNumberButtonPressed(string number)
    {
        if (isProcessing) return; // ������ڴ����������������

        // ���ƴ𰸳��ȣ����3λ����
        if (currentAnswer.Length < 3)
        {
            currentAnswer += number;
            UpdateTitleDisplay();
        }
    }

    /// <summary>
    /// �˸�ť�����¼�
    /// </summary>
    void OnBackspaceButtonPressed()
    {
        if (isProcessing) return; // ������ڴ����������������

        if (currentAnswer.Length > 0)
        {
            currentAnswer = currentAnswer.Substring(0, currentAnswer.Length - 1);
            UpdateTitleDisplay();
        }
    }

    /// <summary>
    /// ȷ�ϰ�ť�����¼�
    /// </summary>
    void OnConfirmButtonPressed()
    {
        if (isProcessing || string.IsNullOrEmpty(currentAnswer)) return;

        isProcessing = true;
        int answer = int.Parse(currentAnswer);

        // �жϴ��Ƿ���ȷ
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
    /// ��ʾ���ⷴ��
    /// </summary>
    /// <param name="isCorrect">���Ƿ���ȷ</param>
    void ShowFeedback(bool isCorrect)
    {
        // ��ʾ��Ӧ��ͼ��
        if (isCorrect)
        {
            if (right != null)
            {
                right.gameObject.SetActive(true);
                // ��Ӷ���Ч��
                right.transform.localScale = Vector3.zero;
                right.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            if (wrong != null)
            {
                wrong.gameObject.SetActive(true);
                // ��Ӷ���Ч��
                wrong.transform.localScale = Vector3.zero;
                wrong.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }

        // ���µ÷�
        if (isCorrect)
        {
            score += 10; // ÿ��10��
            Debug.Log($"����ȷ����ǰ�÷�: {score}");
            explan.text = $"��ȷ";
        }
        else
        {
            explan.text = $"����,��ȷ����: {correctAnswer}";
            Debug.Log($"�𰸴�����ȷ����: {correctAnswer}");
        }

        progress.text = $"�÷�{score}/{100}({currentQuestion * 10}%)";
        
        // 3��������һ��
        DOVirtual.DelayedCall(1.0f, () => {
            explan.text = $"���Ĵ�";
            ansure.text = "_";
            GenerateNextQuestion();
        });
    }

    /// <summary>
    /// ���ط���ͼ��
    /// </summary>
    void HideFeedbackIcons()
    {
        if (right != null) right.gameObject.SetActive(false);
        if (wrong != null) wrong.gameObject.SetActive(false);
    }

    /// <summary>
    /// ������Ŀ��ʾ
    /// </summary>
    void UpdateTitleDisplay()
    {
        //title.text = $"��{currentQuestion}��: {questionText} = {currentAnswer}";
        ansure.text = $"{currentAnswer}";
    }
    
    /// <summary>
    /// ������ϰ
    /// </summary>
    void EndPractice()
    {
        HideFeedbackIcons();
        progress.text = $"��ϰ�������ܵ÷�: {score}/100";
        Debug.Log($"��ϰ�������ܵ÷�: {score}/100");
        Main.DispEvent("event_over", new object[] { score, 30 });
        // ����������������������߼���������ʾ���۵�
    }
}