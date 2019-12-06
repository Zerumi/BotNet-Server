// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
namespace CommandsLibrary
{
    public class Arguments : IArgument
    {
        public string Command { get; set; }
        public int ArgumentCount { get; set; }
        public string[] ArgumentsList { get ; set; }

        public static IArgument[] arguments =
        {
            new Arguments()
            {
                Command = "/create",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                }
            },
            new Arguments()
            {
                Command = "/delete",
                ArgumentCount =  1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                }
            },
            new Arguments()
            {
                Command = "/copy",
                ArgumentCount = 2,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл для копирования",
                    "Путь к файлу\\Файл для вставки"
                }
            },
            new Arguments()
            {
                Command = "/start",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                }
            },
            new Arguments()
            {
                Command = "/startinvis",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                }
            },
            new Arguments()
            {
                Command = "/nameofpc",
                ArgumentCount = 0,
                ArgumentsList = null
            },
            new Arguments()
            {
                Command = "/screen",
                ArgumentCount = 0,
                ArgumentsList = null
            }
        };
    }
}
