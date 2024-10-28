using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DL_COMMAND_DATA 
{
    private const char COMMANDSPLITTER_ID = ',';
    private const char ARGUMENTSCONTAINER_ID = '(';

    public List<Command> commands;
    public struct Command
    {
        public string name;
        public string[] arguments;
    }

    public DL_COMMAND_DATA(string rawCommands)
    {
        commands = RipCommands(rawCommands);
    }

    public List<Command> RipCommands(string rawCommands)
    {
        string[] data = rawCommands.Split(COMMANDSPLITTER_ID, System.StringSplitOptions.RemoveEmptyEntries);
        var result = new List<Command>();

        foreach (string cmd in data)
        {
            var command = new Command();
            int index = cmd.IndexOf(ARGUMENTSCONTAINER_ID);
            command.name = cmd.Substring(0, index).Trim();
            //PlaySong("SongName" - v 1 - p 1)
            command.arguments = GetArgs(cmd.Substring(index + 1, cmd.Length - index - 2));
            result.Add(command);
        }

        return result; 
    }

    private string[] GetArgs(string args) {

        var argList = new List<string>();
        var currentArg = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (!inQuotes && args[i] == ' ') {
                argList.Add(currentArg.ToString());
                currentArg.Clear();
                continue;
            }

            currentArg.Append(args[i]);
        }

        if (currentArg.Length > 0)
            argList.Add(currentArg.ToString());

        return argList.ToArray();
    }


}
