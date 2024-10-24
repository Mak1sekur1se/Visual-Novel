using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TESTING
{
    public class TestDialogueFiles : MonoBehaviour
    {
        private void Start()
        {
            StartConversation();
        }

        private void StartConversation()
        {
            List<string> lines = FileManagers.ReadTextAsset("testFile", false);

            DialogueSystem.Instance.Say(lines);
        }
    }
}
