using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitect 
{
    private TextMeshProUGUI tmproUi;//2D
    private TextMeshPro tmproWorld;//3D

    public TMP_Text tmpro => tmproUi != null ? tmproUi : tmproWorld;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";//已有的文本，让打字机可以新加或者可以直接加入
    private int preTextLength = 0;

    public string fullTargetText => preText + targetText;//应该构建的完整文本

    public enum BuildMethod { instant, typewriter, fade}
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    public float speed { get { return BASESPEED * speedMultiplier; } set { speedMultiplier = value; } }
    private const float BASESPEED = 1;
    private float speedMultiplier = 1;

    //点击事件加快文字速度

    //每帧显示的字符数量来达到加快的效果
    public int charactersPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3;  } }
    private int characterMultiplier = 1;

    public bool hurryUp = false;
}
