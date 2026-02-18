using System;
using System.Collections.Generic;

namespace MyFirstPlugin
{
    public class AccountPost
    {
        private string accountName;
        private string accountOwner;

        public AccountPost(string name, string owner)
        {
            accountName = name;
            accountOwner = owner;
        }

        public void CreateTask()
        {
            // Logic to create a task using account details
            var taskSubject = accountName;
            var taskOwner = accountOwner;

            Console.WriteLine($"Task Created: Subject - {taskSubject}, Owner - {taskOwner}");
        }
    }
}