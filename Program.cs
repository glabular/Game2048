using System;


namespace Game2048
{
    public class Program
    {
        static void Main(string[] args)
        {
            var playAgain = false;
            var size = 4;
            Field2048 field2048 = new Field2048(size, size);

            do
            {
                field2048.InitializeField();
                var playersMove = new ConsoleKeyInfo();

                do
                {
                    field2048.Show(true);
                    field2048.MakeMove(playersMove = Console.ReadKey());

                } 
                while (!field2048.GameOver);

                field2048.Show(false);
                Console.WriteLine();
                Console.WriteLine("------------");
                Console.WriteLine($"Счёт: {field2048.Score}");
                Console.WriteLine($"Рекорд: {field2048.HighestScore}");
                Console.WriteLine("------------");
                Console.WriteLine("Игра окончена. Нажмите Enter.");
                Console.ReadLine();
                Console.WriteLine("Нажмите F1 чтобы сыграть ещё раз.");
                Console.WriteLine("Нажмите F2 чтобы выйти");

                var playersChoice = new ConsoleKeyInfo();
                playersChoice = Console.ReadKey();

                if (playersChoice.Key == ConsoleKey.F1)
                {
                    playAgain = true;
                }
                else if (playersChoice.Key == ConsoleKey.F2)
                {
                    playAgain = false;
                }
            } 
            while (playAgain);
        }
    }
}
