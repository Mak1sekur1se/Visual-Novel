using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TESTING
{
    public class TestDialogueFiles : MonoBehaviour
    {
        [SerializeField] private TextAsset textFile = null;
        private void Start()
        {
            StartConversation();
        }

        private void StartConversation()
        {
            List<string> lines = FileManagers.ReadTextAsset(textFile, false);

            //for (int i = 0; i < lines.Count; i++)
            //{
            //    string line = lines[i];

            //    if (string.IsNullOrWhiteSpace(line))
            //        continue;

            //    DIALOGUE_LINE dl = DialogueParser.Parse(line);

            //    Debug.Log($"{dl.speaker.name} as [{(dl.speaker.castName != string.Empty ? dl.speaker.castName : dl.speaker.name)}]at {dl.speaker.castPosition}");

            //    List<(int l, string ex)> expr = dl.speaker.CastExpresstion;
            //    for (int c = 0; c < expr.Count; c++)
            //    {
            //        Debug.Log($"[Layer[{expr[c].l}] = '{expr[c].ex}']");
            //    }
            //}

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                DIALOGUE_LINE dl = DialogueParser.Parse(line);

                for (int i = 0; i < dl.commandData.commands.Count; i++)
                {
                    DL_COMMAND_DATA.Command command = dl.commandData.commands[i];
                    Debug.Log($"Command [{i}] '{command.name}' has arguments [{string.Join(",", command.arguments)}]");
                }

            }

            //DialogueSystem.Instance.Say(lines);
        }
    }
}
