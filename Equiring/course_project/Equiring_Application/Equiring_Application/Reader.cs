using System.IO;

namespace Equiring_Application
{
    /* Данный класс отвечает за поиск файлов в директории */
    class Reader
    {

        /*Функция поиска файлов в директории передаваемой в параметр patch 
          по его имени и маске передаваемой в параметре pattern
         */
        public string[] SearchFile(string patch, string pattern)
        {
            /*Флаг SearchOption.AllDirectories означает искать во всех вложенных папках*/
            string[] ReultSearch = Directory.GetFiles(patch, pattern, SearchOption.AllDirectories);
                        
            return ReultSearch;
        }
    }
}
