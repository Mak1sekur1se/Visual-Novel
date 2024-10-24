using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class TestParsing : MonoBehaviour
    {
        [SerializeField] private TextAsset file;
        private void Start()
        {
            SendFileToParse();
        }

        private void SendFileToParse()
        {
            List<string> lines = FileManagers.ReadTextAsset("testFile", false);

            foreach (string line in lines)
            {
                DIALOGUE_LINE dl = DialogueParser.Parse(line);
            }
        }
    }


}
