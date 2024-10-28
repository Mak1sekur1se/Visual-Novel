using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class DL_DIALOGUE_DATA
{
    //匹配{c}/{a}/{wc decimal}/{wa decimal}
    public const string segmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";

    public List<DIALOGUE_SEGMENT> segments;


    //字符串改为对话数据 speaker "dialogue Line 1{}Line 2{}Line3"
    public DL_DIALOGUE_DATA(string rawDialogue)
    {
        segments = RipSegments(rawDialogue);
    }

    public List<DIALOGUE_SEGMENT> RipSegments(string rawDialogue)
    {
        var segments = new List<DIALOGUE_SEGMENT>();
        MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPattern);


        int lastIndex = 0;
        //找到
        DIALOGUE_SEGMENT segment = new DIALOGUE_SEGMENT();
        // 0 这一行对话没有特殊命令符 找到特殊命令符获取第一个命令符前的字符串
        segment.dialogue = (matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index));
        segment.startSignal = DIALOGUE_SEGMENT.StartSignal.NONE;
        segment.signalDelay = 0;
        segments.Add(segment);

        //没有匹配的返回这一行分裂的所有语句
        if (matches.Count == 0)
            return segments;
        //这一行有特殊命令符更新最后一位
        else
            lastIndex = matches[0].Index;

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            segment = new DIALOGUE_SEGMENT();

            //找到这一分段的特殊命令字符
            string signalMatch = match.Value;//{A}
            signalMatch = signalMatch.Substring(1, match.Length - 2);
            string[] signalSplit = signalMatch.Split(' ');

            segment.startSignal = (DIALOGUE_SEGMENT.StartSignal) Enum.Parse(typeof(DIALOGUE_SEGMENT.StartSignal), signalSplit[0].ToUpper());

            //获取这一分段的signaldelay
            if (signalSplit.Length > 1)
                float.TryParse(signalSplit[1], out segment.signalDelay);

            //获取这一分段的对话
            int nextIndex = i + 1 < matches.Count ? matches[i + 1].Index : rawDialogue.Length;
            segment.dialogue = rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length));
            lastIndex = nextIndex;

            segments.Add(segment);
        }
        return segments;
    }

    public struct DIALOGUE_SEGMENT
    {
        public string dialogue;
        public StartSignal startSignal;
        public float signalDelay;
        public enum StartSignal { NONE, C, A, WA, WC }

        public bool appendText => (startSignal == StartSignal.A || startSignal == StartSignal.WA);
    }
}
