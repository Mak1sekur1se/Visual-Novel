using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DIALOGUE_LINE 
    {
        public DL_SPEAKER_DATA speaker;
        public DL_DIALOGUE_DATA dialogue;
        public string command;

        public bool hasSpeaker => speaker != null;//speaker != string.Empty;
        public bool hasDialogue => dialogue.hasDialogue;
        public bool hasCommand => command != string.Empty;  

        public DIALOGUE_LINE(string speaker, string dialogue, string command)
        {
            this.speaker = (string.IsNullOrWhiteSpace(speaker) ? null : new DL_SPEAKER_DATA(speaker));
            this.dialogue = new DL_DIALOGUE_DATA(dialogue);
            this.command = command;
        }
    }
}
