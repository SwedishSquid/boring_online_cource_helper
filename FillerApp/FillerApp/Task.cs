using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FillerApp
{
    public class Task
    {
        public string TaskText {get; set;}
        public string[] AnswerVariants { get; set;}
        public string[] TrueAnswers { get; set;}

        public Task(string TaskText, IEnumerable<string> AnswerVariants, IEnumerable<string> TrueAnswers)
        {
            this.TaskText = TaskText;
            this.AnswerVariants = AnswerVariants.ToArray();
            this.TrueAnswers = TrueAnswers.ToArray();
        }

        public Task()
        {

        }

        public static Task ActiveParse()
        {
            Console.WriteLine("input task in format 'task text' === 'answer vars separated by next line' === 'true answers' ===");
            var taskTextLines = new List<string>();
            while (true)
            {
                var possibleTaskTextLine = Console.ReadLine().Trim();
                if (possibleTaskTextLine.StartsWith("==="))
                {
                    break;
                }
                if (possibleTaskTextLine.Length > 0)
                {
                    taskTextLines.Add(possibleTaskTextLine);
                }
            }

            var answerVariants = new List<string>();
            while (true)
            {
                var possibleAnsVar = Console.ReadLine().Trim();
                if (possibleAnsVar.StartsWith("==="))
                {
                    break;
                }
                if (possibleAnsVar.Length > 0)
                {
                    answerVariants.Add(possibleAnsVar);
                }
            }
            
            var trueAnswers = new List<string>();
            var nums = Console.ReadLine().Trim().Split(' ').Select(s => int.Parse(s));
            foreach (var num in nums)
            {
                trueAnswers.Add(answerVariants[num]);
            }

            return new Task(string.Join('\n', taskTextLines), answerVariants, trueAnswers);
        }
    }
}
