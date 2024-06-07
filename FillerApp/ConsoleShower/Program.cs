using FillerApp;
using System.Collections.Frozen;
using System.Security.Cryptography;

namespace ConsoleShower
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filepath = Constants.HistoryBasePath;
            var taskList = Utils.ReadTasksFromFile(filepath);
            Start(taskList);
        }

        public static void Start(List<FillerApp.Task> taskList)
        {
            Console.WriteLine("choose mode: 'nonshuffle' 'shuffle'");
            var mode = Console.ReadLine();
            if (mode == "shuffle")
            {
                Shuffle(taskList);
                ShowAll(taskList);
                return;
            }
            if (mode == "nonshuffle")
            {
                ShowAll(taskList, 0);
                return;
            }

            throw new Exception($"mode {mode} is invalid");
        }

        private static Random rng = new Random();
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
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
                Console.Write("press any button to continue");
                Console.ReadKey();
                Console.WriteLine();
            }
        }

        private static void RenderTask(FillerApp.Task task)
        {
            Console.WriteLine();
            Console.WriteLine("-=-=-=-=-=--=-=-=-=-=--=-=-=--=-=-=--=-==-==-=-=-==-=-=-=-=-=");
            Console.WriteLine(task.TaskText);
            Console.WriteLine("------- Options: --------");
            Shuffle(task.AnswerVariants);
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
                    Console.WriteLine("------------------------------------------------full ans +1");
                    return true;
                }
                Console.WriteLine("------------------------------------------------wrong");
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
