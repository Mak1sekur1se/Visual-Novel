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
                dialogueSystem.ShowSpeakerName(line.speakerData.castName);
            //˵�������л����в��������ļ�ÿһ�ж���˵��������

            yield return BuildLineSegments(line.dialogueData);

            //�ȴ��û���ʾ�����л�����һ��
            yield return WaitForUserInput();

        }
        private IEnumerator Line_RunCcommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commandData);
            yield return null;
        }

        private IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line )
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTrigger(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        private IEnumerator WaitForDialogueSegmentSignalToBeTrigger(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    //CAֱ�ӵȴ�����
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                    //�ȴ�
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default: break;
            }
        }

        private IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            if (!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);
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
