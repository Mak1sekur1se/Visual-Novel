using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DIALOGUE_LINE 
    {
        public DL_SPEAKER_DATA speakerData;
        public DL_DIALOGUE_DATA dialogueData;
        public DL_COMMAND_DATA commandData;

        public bool hasSpeaker => speakerData != null;//speaker != string.Empty;
        public bool hasDialogue => dialogueData != null;
        public bool hasCommand => commandData != null;  

        public DIALOGUE_LINE(string speaker, string dialogue, string command)
        {
            this.speakerData = (string.IsNullOrWhiteSpace(speaker) ? null : new DL_SPEAKER_DATA(speaker));
            this.dialogueData =(string.IsNullOrWhiteSpace(speaker) ? null :new DL_DIALOGUE_DATA(dialogue));
            this.commandData = (string.IsNullOrWhiteSpace(command) ? null : new DL_COMMAND_DATA(command)) ;
        }
    }
}
