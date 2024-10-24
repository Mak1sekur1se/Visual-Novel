using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueParser 
    {
        private const string commandRegexPattern = "\\w*[^\\s]\\(";//ʹ���ַ���Ҫ\\w
        public static DIALOGUE_LINE Parse(string rawLine)
        {
            Debug.Log($"Parsing line - '{rawLine}'");

            (string speaker, string dialogue, string command) = RipContent(rawLine);

            Debug.Log($"Speaker is '{speaker}'\ndialogue is '{dialogue}'\ncommand is '{command}'");

            return new DIALOGUE_LINE(speaker, dialogue, command);

        }

        private static (string, string, string) RipContent(string rawLine)
        {
            //Ŀ���ַ���1 Speaker "Dialogue \"Goes\" Here" Command(arguments go here)
            //Ŀ���ַ���2 Elen "Let's listen to some music!" PlaySong("Ethereal Presence" -v 1 -p 0.3)
            
            string speaker = string.Empty, dialogue = string.Empty, commands = string.Empty;

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            //���Ŀ���ַ����жԻ����֣��ܹ�ʶ��ת���
            for (int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\')
                    //��ǰ�ַ���ת���
                    isEscaped = !isEscaped;
                else if (current == '"' && !isEscaped)
                {
                    //��ǰ�ַ���������ǰһ������ת���
                    if (dialogueStart == -1)
                        //�ǶԻ���ʼ��
                        dialogueStart = i;
                    else if (dialogueEnd == -1)
                        //�ǶԻ��յ�
                        dialogueEnd = i;
                }
                else
                    //��ǰ�ַ��Ȳ���ת����ֲ�������
                    isEscaped = false;
            }
            ////Start���������Լ�һ��ȡ������Ч������ -1 ȥ��ĩβ������
            //Debug.Log(rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1));

            //ʶ��������
            Regex commandRegex = new Regex(commandRegexPattern);
            Match match = commandRegex.Match(rawLine);

            int commandStart = -1;
            if (match.Success)
            {
                commandStart = match.Index;
                //String.Trim �Ƴ���ͷ��β�Ŀհ��ַ�
                if (dialogueStart == -1 && dialogueEnd == -1)
                    //��һ����û�жԻ��Ĵ�������
                    return ("", "", rawLine.Trim());
            }

            //Ҫô�ǶԻ���Ҫô���������еĶ���ʲ������ֱ�
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                //��һ����˵������˵������������ľ��Ӽ���
                speaker = rawLine.Substring(0, dialogueStart).Trim() ;
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"", "\"") ;//�滻��ת���
                if (commandStart != -1)
                    commands = rawLine.Substring(commandStart).Trim();
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
                //������
                commands = rawLine;
            
            else
                speaker = rawLine;


            return (speaker, dialogue, commands);
        }
    }
}
