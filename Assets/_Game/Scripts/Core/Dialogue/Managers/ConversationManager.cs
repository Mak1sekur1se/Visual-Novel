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
        //用户加速
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
                //不处理空文字行
                if (string.IsNullOrWhiteSpace(conversation[i])) 
                    continue;

                //解析字符串
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //展示对话
                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                }

                //运行命令
                if (line.hasCommand)
                {
                    yield return Line_RunCcommands(line);
                }

                //等待一秒 切换为手动
                //yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //更新对话框展示或隐藏对话者名称
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);
            else
                dialogueSystem.HideSpeakerName();

            yield return BuildDialogue(line.dialogue);

            //等待用户提示输入切换到下一行
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
                //对话系统通知对话管理换行
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
