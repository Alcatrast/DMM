using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM_Server
{
    class Base
    {
        private
         List<Account> accounts = new List<Account>();
        private
         string currentId;
        private
         bool isOpened;
        private
         int baseNum;

        /// <summary>
        /// База на 999 пользователей.
        /// </summary>
        /// <param name="index"> Первые цифры индекса пользователей в базе.</param>
        public
         Base(int index)
        {
            this.baseNum = index;
            this.currentId = Convert.ToString(index) + "000";
            this.isOpened = true;
        }
        /// <summary>
        /// Открыта ли база.
        /// </summary>
        /// <returns></returns>
        public
         bool IsOpened()
        { return this.isOpened; }
        private
         void NextID()
        {
            this.currentId = Convert.ToString(Convert.ToInt32(this.currentId) + 1);
            if ((Convert.ToInt32(this.currentId) + 1) > (this.baseNum) * 1000 + 999)
            {
                this.isOpened = false;
            }
        }

        /// <summary>
        /// Добавляет пользователя в базу и возвращает его ID.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns> "-1" - провал.</returns>
        public
         string AddAccount(string login, string password)
        {
            if ((SetDataIsCorrect(login) && SetDataIsCorrect(password)) == false)
            {
                return "-1";
            }
            if (Convert.ToBoolean(this.isOpened))
            {
                NextID();

                accounts.Add(new Account(
                    currentId, login, password)); /*записать новый аккаунт в файл базы;*/
                return currentId;
            }
            else
            {
                return "-1";
            }
        }

        /// <summary>
        /// Возвращает индес пользователя с введенным ID в базе.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public
         int FindByID(string ID)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if ((Convert.ToString(accounts[i].ID())) == ID) return i;
            }
            return -1;
        }

        /// <summary>
        /// Возвращает индес пользователя с введенным логином в базе.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public
         int FindByLogin(string login)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if ((Convert.ToString(accounts[i].Login())) == login) return i;
            }
            return -1;
        }

        /// <summary>
        /// Обозначает контракт на доступ позьзователя к своим данным, возвращает его
        /// индекс в базе по его ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="password"></param>
        /// <returns>-1 - провал</returns>
        public
         int AccessByID(string ID, string password)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if ((Convert.ToString(accounts[i].ID())) == ID) { if (accounts[i].IsPasswordCorrect(password)) { return i; } else { return -2; } };
            }
            return -1;
        }

        /// <summary>
        /// Обозначает контракт на доступ позьзователя к своим данным, возвращает его
        /// индекс в базе по его логину.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>-1 аккаута не существует.\n -2 указанный пароль неверный.</returns>
        public
         int AccessByLogin(string login, string password)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                if ((Convert.ToString(accounts[i].Login())) == login) { if (accounts[i].IsPasswordCorrect(password)) { return i; } else { return -2; } };
            }
            return -1;
        }

        /// <summary>
        ///Обращение к аккаунту в базе по его индексу в ней.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public
         Account Item(int index)
        {
            Console.WriteLine(index);
            return accounts[index];
        }

        private
         bool SetDataIsCorrect(string data)
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