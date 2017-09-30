using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.TaskScheduler;

namespace AntiGFW {
    internal static class TaskScheduler {
        internal static void CreateTask() {
            DateTime dateTime, now = DateTime.Now;
            dateTime = new DateTime(now.Year, now.Month, now.Day - 1).AddSeconds(10);
            TaskService taskService = new TaskService();
            ExecAction execAction = new ExecAction(Utils.ExePath, null, Utils.ExeDirectory);
            TaskDefinition taskDefinition = taskService.NewTask();
            taskDefinition.RegistrationInfo.Description = "AntiGFW";
            taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
            DailyTrigger dailyTrigger = new DailyTrigger();
            dailyTrigger.StartBoundary = dateTime;
            dailyTrigger.Repetition.Duration = TimeSpan.Zero;
            dailyTrigger.Repetition.Interval = TimeSpan.FromHours(1);
            taskDefinition.Triggers.Add(dailyTrigger);
            taskDefinition.Actions.Add(execAction);
            taskService.RootFolder.DeleteTask("AntiGFW", false);
            taskService.RootFolder.RegisterTaskDefinition("AntiGFW", taskDefinition);
        }
    }
}
