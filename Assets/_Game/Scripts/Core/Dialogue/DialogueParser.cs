using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueParser 
    {
        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";//使用字符需要\\w
        public static DIALOGUE_LINE Parse(string rawLine)
        {
            Debug.Log($"Parsing line - '{rawLine}'");

            (string speaker, string dialogue, string command) = RipContent(rawLine);

            Debug.Log($"Speaker is '{speaker}'\ndialogue is '{dialogue}'\ncommand is '{command}'");

            return new DIALOGUE_LINE(speaker, dialogue, command);

        }

        private static (string, string, string) RipContent(string rawLine)
        {
            //目标字符串1 Speaker "Dialogue \"Goes\" Here" Command(arguments go here)
            //目标字符串2 Elen "Let's listen to some music!" PlaySong("Ethereal Presence" -v 1 -p 0.3)

            string speaker = string.Empty, dialogue = string.Empty, commands = string.Empty;

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            //获得目标字符串中对话部分，能够识别转义符
            for (int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\')
                    //当前字符是转义符
                    isEscaped = !isEscaped;
                else if (current == '"' && !isEscaped)
                {
                    //当前字符有引号且前一个不是转义符
                    if (dialogueStart == -1)
                        //是对话起始点
                        dialogueStart = i;
                    else if (dialogueEnd == -1)
                        //是对话终点
                        dialogueEnd = i;
                }
                else
                    //当前字符既不是转义符又不是引号
                    isEscaped = false;
            }
            ////Start是引号所以加一读取后面有效的数据 -1 去掉末尾的引号
            //Debug.Log(rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1));

            //识别命令行
            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawLine);

            int commandStart = -1;
            foreach (Match match in matches)
            {
                if (match.Index < dialogueStart || match.Index > dialogueEnd)
                {
                    commandStart = match.Index;
                    break;
                }
            }

            if (commandStart != -1 && (dialogueStart == -1 && dialogueEnd == -1))
                return ("", "", rawLine.Trim());

            //要么是对话框要么是命令行中的多个词参数，分辨
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                //是一个有说话人有说话内容有命令的句子集合
                speaker = rawLine.Substring(0, dialogueStart).Trim() ;
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"", "\"") ;//替换掉转义符
                if (commandStart != -1)
                    commands = rawLine.Substring(commandStart).Trim();
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
                //命令行
                commands = rawLine;
            
            else
                dialogue = rawLine;


            return (speaker, dialogue, commands);
        }
    }
}
