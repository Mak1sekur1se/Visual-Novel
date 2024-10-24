using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DIALOGUE_LINE 
    {
        public string speaker;
        public string dialogue;
        public string command;

        public bool hasSpeaker => speaker != string.Empty;
        public bool hasDialogue => dialogue != string.Empty;
        public bool hasCommand => command != string.Empty;  

        public DIALOGUE_LINE(string speaker, string dialogue, string command)
        {
            this.speaker = speaker;
            this.dialogue = dialogue;
            this.command = command;
        }
    }
}
