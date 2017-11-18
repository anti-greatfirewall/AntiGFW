using System;
using Microsoft.Win32.TaskScheduler;

namespace AntiGFW {
    internal static class TaskScheduler {
        private static readonly TaskService taskService = new TaskService();
        
        internal static void DeleteTask() {
            taskService.RootFolder.DeleteTask("AntiGFW", false);
        }

        internal static void CreateTask() {
            DateTime now = DateTime.Now;
            DateTime dateTime = new DateTime(now.Year, now.Month, now.Day - 1).AddSeconds(10);
            ExecAction execAction = new ExecAction(Utils.ExePath, null, Utils.ExeDirectory);
            TaskDefinition taskDefinition = taskService.NewTask();
            taskDefinition.RegistrationInfo.Description = "AntiGFW";
            taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
            DailyTrigger dailyTrigger = new DailyTrigger {
                StartBoundary = dateTime,
                Repetition = {
                    Duration = TimeSpan.Zero,
                    Interval = TimeSpan.FromHours(1)
                }
            };
            taskDefinition.Triggers.Add(dailyTrigger);
            taskDefinition.Actions.Add(execAction);
            taskService.RootFolder.DeleteTask("AntiGFW", false);
            taskService.RootFolder.RegisterTaskDefinition("AntiGFW", taskDefinition);
        }

        internal static bool FindTask() {
            return taskService.RootFolder.GetTasks().Exists("AntiGFW");
        }
    }
}
