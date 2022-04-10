using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM_Server
{
    class Account
    {
        private
         string id;
        private
         string login;
        private
         string password;
        private
         string address;
        private
         List<string> activeInterlocutors = new List<string>();

        public
         Account()
        { }
        public
         Account(string id, string login, string pasword)
        {
            this.login = login;
            this.password = pasword;
            this.id = id;
        }

        public
         string ID()
        { return this.id; }

        public
         string Login()
        { return this.login; }

        public
         string Address()
        { return this.address; }

        /// <summary>
        /// Обновление IP адреса вместе с портом.
        /// </summary>
        /// <param name="address"></param>
        public
         void UpdateAddress(string address)
        { this.address = address; }

        /// <summary>
        /// Обновление логина.
        /// </summary>
        /// <param name="login"></param>
        public
         bool UpdateLogin(string newLogin)
        { if (SetDataIsCorrect(newLogin)) { this.login = newLogin; return true; } else { return false; } }

        /// <summary>
        /// Обновление пароля.
        /// </summary>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns> True - успех.</returns>
        public
         bool UpdatePassword(string currentPassword, string newPassword)
        {
            if (currentPassword == this.password && SetDataIsCorrect(newPassword))
            {
                this.password = newPassword;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет актуальность введенного пароля.
        /// </summary>
        /// <param name="currentPassword"></param>
        /// <returns></returns>
        public
         bool IsPasswordCorrect(string currentPassword)
        {
            return this.password == currentPassword;
        }

        /// <summary>
        /// Добавляет в список человека, у которого не получилось установить прямое
        /// соединение с текущим собеседником.
        /// </summary>
        /// <param name="currentInterlocutor">ID человека.</param>
        /// <returns> True - если список успешно пополнен. \n False - если этот ID уже
        /// есть в списке.</returns>
        public
         bool AddActiveInterlocutor(string currentInterlocutor)
        {
            if (activeInterlocutors.Contains(currentInterlocutor) == false)
            {
                activeInterlocutors.Add(currentInterlocutor);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Единожды возврвщает список людей, сообщения которых не дошли напрямую,
        /// после чего удаляет список.
        /// </summary>
        /// <returns> Список ID.</returns>
        public
         List<string> FindActiveInterlocutors()
        {
            List<string> buffer = new List<string>();
            for (int i = 0; i < activeInterlocutors.Count; i++)
            {
                buffer.Add(activeInterlocutors[i]);
            }
            activeInterlocutors.Clear();
            return buffer;
        }

        private bool SetDataIsCorrect(string data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '\'' || data[i] == '\"' || data[i] == '\n' ||
                    data[i] == '\r' || data[i] == '\0' || data[i] == '<' ||
                    data[i] == '>' || data[i] == ' ' || data[i] == '\\')
                {
                    return false;
                }
            }
            return true;
        }
    }
}  // namespace DMM_Server