using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class DL_DIALOGUE_DATA
{
    //ƥ��{c}/{a}/{wc decimal}/{wa decimal}
    public const string segmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";

    public List<DIALOGUE_SEGMENT> segments;


    //�ַ�����Ϊ�Ի����� speaker "dialogue Line 1{}Line 2{}Line3"
    public DL_DIALOGUE_DATA(string rawDialogue)
    {
        segments = RipSegments(rawDialogue);
    }

    public List<DIALOGUE_SEGMENT> RipSegments(string rawDialogue)
    {
        var segments = new List<DIALOGUE_SEGMENT>();
        MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPattern);


        int lastIndex = 0;
        //�ҵ�
        DIALOGUE_SEGMENT segment = new DIALOGUE_SEGMENT();
        // 0 ��һ�жԻ�û����������� �ҵ������������ȡ��һ�������ǰ���ַ���
        segment.dialogue = (matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index));
        segment.startSignal = DIALOGUE_SEGMENT.StartSignal.NONE;
        segment.signalDelay = 0;
        segments.Add(segment);

        //û��ƥ��ķ�����һ�з��ѵ��������
        if (matches.Count == 0)
            return segments;
        //��һ��������������������һλ
        else
            lastIndex = matches[0].Index;

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            segment = new DIALOGUE_SEGMENT();

            //�ҵ���һ�ֶε����������ַ�
            string signalMatch = match.Value;//{A}
            signalMatch = signalMatch.Substring(1, match.Length - 2);
            string[] signalSplit = signalMatch.Split(' ');

            segment.startSignal = (DIALOGUE_SEGMENT.StartSignal) Enum.Parse(typeof(DIALOGUE_SEGMENT.StartSignal), signalSplit[0].ToUpper());

            //��ȡ��һ�ֶε�signaldelay
            if (signalSplit.Length > 1)
                float.TryParse(signalSplit[1], out segment.signalDelay);

            //��ȡ��һ�ֶεĶԻ�
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
