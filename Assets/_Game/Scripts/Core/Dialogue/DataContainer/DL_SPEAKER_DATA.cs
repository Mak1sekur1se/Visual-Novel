using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DL_SPEAKER_DATA
{
    //真实名称和显示名称
    public string name, castName;

    //屏幕上显示的名字
    public string displayname => (castName != string.Empty ? castName : name);

    public Vector2 castPosition;
    public List<(int layer, string expresstion)> CastExpresstion { get; set; }

    private const string NAMECAST_ID = " as ";
    private const string POSITIONCAST_ID = " at ";
    private const string EXPRESSIONCAST_ID = " [";
    private const char AXISDELIMITER = ':';
    private const char EXPRESSIONLAYER_JOINER = ',';
    private const char EXPRESSIONLAYER_DELIMITER = ':';

    public DL_SPEAKER_DATA(string rawSpeaker)
    {
        string pattern = @$"{NAMECAST_ID}|{POSITIONCAST_ID}|{EXPRESSIONCAST_ID.Insert(EXPRESSIONCAST_ID.Length - 1, @"\")}";
        MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

        //避免空引用
        castName = "";
        castPosition = Vector2.zero;
        CastExpresstion = new List<(int layer, string expression)>();

        if (matches.Count == 0)
        {
            //纯名字没有额外的设定
            name = rawSpeaker;
            return;
        }

        //处理有特殊命令的Speaker
        int index = matches[0].Index;

        name = rawSpeaker.Substring(0, index);

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            int startIndex = 0, endIndex = 0;

            if (match.Value == NAMECAST_ID)
            {
                //原始字符串中的起始位置 只要as后面的
                startIndex = match.Index + NAMECAST_ID.Length;
                endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;

                //有个空格
                castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
            }
            else if (match.Value == POSITIONCAST_ID)
            {
                startIndex = match.Index + POSITIONCAST_ID.Length;
                endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                string castPos = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                string[] axis = castPos.Split(AXISDELIMITER, System.StringSplitOptions.RemoveEmptyEntries);

                float.TryParse(axis[0], out castPosition.x);

                if (axis.Length > 1)
                    float.TryParse(axis[1], out castPosition.y);
            }
            else if (match.Value == EXPRESSIONCAST_ID)
            {
                startIndex = match.Index + EXPRESSIONCAST_ID.Length;
                endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : rawSpeaker.Length;
                string castExp = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));//删掉末尾的]

                CastExpresstion = castExp.Split(EXPRESSIONLAYER_JOINER)
                    .Select(x => 
                    {
                        var parts = x.Trim().Split(EXPRESSIONLAYER_DELIMITER);
                        return (int.Parse(parts[0]), parts[1]);
                    }).ToList();
            }
        }

    }
}
