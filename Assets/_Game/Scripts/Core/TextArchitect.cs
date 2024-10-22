using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitect 
{
    private TextMeshProUGUI tmpro_ui;//2D
    private TextMeshPro tmpro_world;//3D

    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";//���е��ı����ô��ֻ������¼ӻ��߿���ֱ�Ӽ���
    private int preTextLength = 0;

    public string fullTargetText => preText + targetText;//Ӧ�ù����������ı�

    //typeWirteһ��һ����ʾ�ɼ����ַ��� fade���ǿɼ��𽥵���͸����
    public enum BuildMethod { instant, typewriter, fade}
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    public float speed { get { return BASESPEED * speedMultiplier; } set { speedMultiplier = value; } }
    private const float BASESPEED = 1;
    private float speedMultiplier = 1;

    //����¼��ӿ������ٶ�

    //ÿ֡��ʾ���ַ��������ﵽ�ӿ��Ч��
    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3;  } }
    private int characterMultiplier = 1;

    public bool hurryUp = false;

    public TextArchitect(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui;
    }

    public TextArchitect(TextMeshPro tmpro_world)
    {
        this.tmpro_world = tmpro_world;
    }

    /// <summary>
    /// һ�仰ֱ����ӽ���
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }
    
    /// <summary>
    /// һ��һ���ֵ���ӽ���
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    public void Stop()
    {
        if (!isBuilding) return;

        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;

    }

    IEnumerator Building()
    {
        Prepare();

        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_TypeWriter();
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }

        OnComplete();
    }

    private void OnComplete()
    {
        //������ɵ��߼�
        buildProcess = null;
        hurryUp = false;
    }

    public void ForeceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                break;
        }

        Stop();
        OnComplete();
    }

    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_TypeWriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;//ǿ�Ƴ�ʼ������
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();//ǿ�Ƹ����ı�����
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }
    private void Prepare_TypeWriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;
        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }
    private void Prepare_Fade()
    {
    }


    //���ֻ�ͨ�����������ӻ��ַ�
    private IEnumerator Build_TypeWriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount) {
            tmpro.maxVisibleCharacters += hurryUp ? charactersPerCycle * 5 : charactersPerCycle;

            yield return new WaitForSeconds(0.015f / speed);//Э����ҪΪ�˸�һ��ʱ���³���
        }
    }

    private IEnumerator Build_Fade()
    {
        yield return null;
    }


}
