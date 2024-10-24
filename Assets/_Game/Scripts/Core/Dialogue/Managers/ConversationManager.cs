using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.Instance;
        private Coroutine process = null;
        public bool isRunning => process != null;

        public TextArchitect architect = null;
        //�û�����
        private bool userPrompt = false;

        public ConversationManager(TextArchitect textArchitect)
        {
            this.architect = textArchitect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public void StartConversation(List<string> conversationn)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversationn));
        }

        public void StopConversation()
        {
            if (!isRunning) 
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        private IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                //�������������
                if (string.IsNullOrWhiteSpace(conversation[i])) 
                    continue;

                //�����ַ���
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //չʾ�Ի�
                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                }

                //��������
                if (line.hasCommand)
                {
                    yield return Line_RunCcommands(line);
                }

                //�ȴ�һ�� �л�Ϊ�ֶ�
                //yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //���¶Ի���չʾ�����ضԻ�������
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);
            else
                dialogueSystem.HideSpeakerName();

            yield return BuildDialogue(line.dialogue);

            //�ȴ��û���ʾ�����л�����һ��
            yield return WaitForUserInput();

        }
        private IEnumerator Line_RunCcommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.command);
            yield return null;
        }

        private IEnumerator BuildDialogue(string dialogue)
        {
            architect.Build(dialogue);

            while (architect.isBuilding)
            {
                //�Ի�ϵͳ֪ͨ�Ի�������
                if (userPrompt)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForeceComplete();

                    userPrompt = false;
                }
                yield return null;
            }
        }

        private IEnumerator WaitForUserInput()
        {
            while(!userPrompt)
                yield return null;
            userPrompt = false;
        }
    }
}
