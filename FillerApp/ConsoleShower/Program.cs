using FillerApp;
using System.Collections.Frozen;

namespace ConsoleShower
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filepath = Constants.HistoryBasePath;
            var taskList = Utils.ReadTasksFromFile(filepath);
            ShowAll(taskList);
        }

        public static void ShowAll(List<FillerApp.Task> tasks, int startIndex = 0)
        {
            var rightAnsCow = 0;
            for (int i = startIndex; i < tasks.Count; i++)
            {
                var task = tasks[i];
                RenderTask(task);
                if (AcceptAnswer(task))
                {
                    rightAnsCow++;
                }

                Console.WriteLine("^^^^^ right ans ^^^^^^^");
                foreach (var rightAns in task.TrueAnswers)
                {
                    Console.WriteLine(rightAns);
                }
                Console.WriteLine($"++++++ {rightAnsCow} / {i + 1 - startIndex} wich is {100 * rightAnsCow / (i + 1.0 - startIndex)} %   +++++");
                Console.ReadKey();
            }
        }

        private static void RenderTask(FillerApp.Task task)
        {
            Console.WriteLine("-=-=-=-=-=--=-=-=-=-=--=-=-=--=-=-=--=-==-==-=-=-==-=-=-=-=-=");
            Console.WriteLine(task.TaskText);
            Console.WriteLine("------- Options: --------");
            for (int j = 0; j < task.AnswerVariants.Length; j++)
            {
                Console.WriteLine($"{j} : {task.AnswerVariants[j]}");
            }
            Console.WriteLine("****** your answer *******");
        }

        private static bool AcceptAnswer(FillerApp.Task task)
        {
            try
            {
                var ansLine = Console.ReadLine();
                var ans = ansLine.Split(' ')
                    .Select(s => int.Parse(s))
                    .Select(ind => task.AnswerVariants[ind])
                    .ToFrozenSet();

                foreach (var a in ans)
                {
                    Console.WriteLine(a);
                }

                if (ans.SetEquals(task.TrueAnswers.ToFrozenSet()))
                {
                    Console.WriteLine("full ans +1");
                    return true;
                }
                Console.WriteLine("wrong");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("assuming you failed this task");
                return false;
            }
        }
    }
}
