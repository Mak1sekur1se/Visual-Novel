using System;
using System.Collections.Generic;
using UnityEngine;
using static DL_COMMAND_DATA;

namespace COMMANS
{

    // A database of all commands that are available for the CommandManager to use
    public class CommandDatabase
    {
        private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

        private bool HasCommand(string commandName) => database.ContainsKey(commandName);

        public void AddCommand(string commandName, Delegate command)
        {
            if (!database.ContainsKey(commandName))
            {
                database.Add(commandName, command);
            }
            else
                Debug.LogError($"Command already exist in the database '{commandName}'");
        }

        public Delegate GetCommand(string commandName)
        {
            if (!database.ContainsKey(commandName))
            {
                Debug.LogError($"Command '{commandName}'does not exist in the database");
                return null;
            }
            return database[commandName];
        }
    }
}