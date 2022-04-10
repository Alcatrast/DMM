using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM_Server
{
    internal class GlobalBase
    {
        private
         List<Base> bases = new List<Base>();

        public
         GlobalBase()
        { bases.Add(new Base(1)); }
        private
         void AddBase()
        { bases.Add(new Base(bases.Count)); }
        private
         string AddAccount(string login, string password)
        {
            string ID;
            if (bases[(bases.Count) - 1].IsOpened() == true)
            {
                ID = bases[(bases.Count) - 1].AddAccount(login, password);
                return ID;
            }
            else
            {
                AddBase();
                ID = bases[(bases.Count) - 1].AddAccount(login, password);
                return ID;
            }
        }
        private
         List<int> FindByLogin(string login)
        {
            int index;
            List<int> ret = new List<int>();
            for (int i = 0; i < bases.Count; i++)
            {
                index = bases[i].FindByLogin(login);
                if (index > -1)
                {
                    ret.Add(i);
                    ret.Add(index);
                    return ret;
                }
            }
            ret.Add(0);
            ret.Add(-1);
            return ret;
        }
        private
         List<int> FindByID(string ID)
        {
            int index;
            List<int> ret = new List<int>();
            for (int i = 0; i < bases.Count; i++)
            {
                index = bases[i].FindByID(ID);
                if (index > -1)
                {
                    ret.Add(i);
                    ret.Add(index);
                    return ret;
                }
            }
            ret.Add(0);
            ret.Add(-1);
            return ret;
        }
        private
         List<int> AcceptByLogin(string login, string password)
        {
            int index;
            List<int> ret = new List<int>();
            for (int i = 0; i < bases.Count; i++)
            {
                index = bases[i].FindByLogin(login);
                if (index > -1)
                {
                    ret.Add(i);
                    if (Convert.ToBoolean(
                            bases[i].Item(index).IsPasswordCorrect(password)))
                    {
                        ret.Add(index);
                    }
                    else
                    {
                        ret.Add(-2);
                    }
                    return ret;
                }
            }
            ret.Add(0);
            ret.Add(-1);
            return ret;
        }
        private
         List<int> AcceptByID(string ID, string password)
        {
            int index;
            List<int> ret = new List<int>();
            for (int i = 0; i < bases.Count; i++)
            {
                index = bases[i].FindByID(ID);
                if (index > -1)
                {
                    ret.Add(i);
                    if (Convert.ToBoolean(
                            bases[i].Item(index).IsPasswordCorrect(password)))
                    {
                        ret.Add(index);
                    }
                    else
                    {
                        ret.Add(-2);
                    }
                    return ret;
                }
            }
            ret.Add(0);
            ret.Add(-1);
            return ret;
        }
        private
         const string cSingUpOrSingIn = "SIOSU";
        private
         const string cFindByLogin = "FBL";
        private
         const string cFindByID = "FBI";
        private
         const string cUpdateLogin = "UL";
        private
         const string cUpdatePassword = "UP";
        private
         const string cAddMeToActiveInterlocutors = "AMTAI";
        private
         const string cCheckMyActiveInterlocutors = "CMAI";

        private
         const string rSuccess = "S";
        private
         const string rFail = "F";
        public
         string Command(string command)
        {
            string[] commandItems = command.Split(' ');
            string ret, iacc;
            List<int> loc1 = new List<int>();
            List<int> loc2 = new List<int>();
            int index = 0;
            bool bb;
            if (commandItems.Length == 4)
            {
                if (commandItems[0] == cSingUpOrSingIn)
                {
                    WriteLog(command);
                    for (int i = 0; i < bases.Count; i++)
                    {
                        index = bases[i].AccessByLogin(commandItems[1], commandItems[2]);
                        if (index == -2)
                        {
                            /*провал - неверный пароль*/
                            ret = rFail + " " + commandItems[2];
                            WriteLog(ret); return ret;
                        }
                        else if (index > -1)
                        {
                            /*успех*/
                            ret = rSuccess + " " + commandItems[1] + " " +
                                  bases[i].Item(index).ID();
                            bases[i].Item(index).UpdateAddress(commandItems[3]);
                            WriteLog(ret); return ret;
                        }
                    }
                    iacc = AddAccount(commandItems[1], commandItems[2]);
                    if (iacc == "-1")
                    {
                        /*провал - не удалось создать аккаунт*/
                        ret = rFail + commandItems[1];
                        WriteLog(ret); return ret;
                    }
                    bases[bases.Count - 1]
                        .Item(bases[bases.Count - 1].FindByID(iacc))
                        .UpdateAddress(commandItems[3]);
                    /*успех*/
                    ret = rSuccess + " " + commandItems[1] + " " + iacc;
                    WriteLog(ret); return ret;
                }
                else if (commandItems[0] == cFindByLogin)
                {
                    WriteLog(command);
                    loc1 = AcceptByID(commandItems[1], commandItems[2]);
                    if (loc1[1] > -1)
                    {
                        loc2 = FindByLogin(commandItems[3]);
                        if (loc2[1] > -1)
                        {
                            /*успех*/
                            ret = rSuccess + " " + bases[loc2[0]].Item(loc2[1]).Login() + " " +
                                  bases[loc2[0]].Item(loc2[1]).ID() + " " +
                                  bases[loc2[0]].Item(loc2[1]).Address();
                            WriteLog(ret); return ret;
                        }
                        else
                        {
                            /*провал - указанного пользователя не существует*/
                            ret = rFail + " " + commandItems[3];
                            WriteLog(ret); return ret;
                        }
                    }
                    else
                    {
                        /*провал - указан неверный пароль*/
                        ret = rFail + " " + commandItems[2];
                        WriteLog(ret); return ret;
                    }
                }
                else if (commandItems[0] == cFindByID)
                {
                    WriteLog(command);
                    loc1 = AcceptByID(commandItems[1], commandItems[2]);
                    if (loc1[1] > -1)
                    {
                        loc2 = FindByID(commandItems[3]);
                        if (loc2[1] > -1)
                        {
                            /*успех*/
                            ret = rSuccess + " " + bases[loc2[0]].Item(loc2[1]).Login() + " " +
                                  bases[loc2[0]].Item(loc2[1]).ID() + " " +
                                  bases[loc2[0]].Item(loc2[1]).Address();
                            WriteLog(ret); return ret;
                        }
                        else
                        {
                            /*провал - указанного пользователя не существует*/
                            ret = rFail + " " + commandItems[3];
                            WriteLog(ret); return ret;
                        }
                    }
                    else
                    {
                        /*провал - указан неверный пароль*/
                        ret = rFail + " " + commandItems[2];
                        WriteLog(ret); return ret;
                    }
                }
                else if (commandItems[0] == cUpdateLogin)
                {
                    WriteLog(command);
                    loc1 = AcceptByID(commandItems[1], commandItems[2]);
                    if (loc1[1] > -1)
                    {
                        bb = bases[loc1[0]].Item(loc1[1]).UpdateLogin(commandItems[3]);
                        if (bb == true)
                        { /*успех*/
                            ret = rSuccess + " " + commandItems[3];
                            WriteLog(ret); return ret;
                        }
                        else
                        { /*некорректные данные*/
                            ret = rFail + " " + commandItems[3];
                            WriteLog(ret); return ret;
                        }
                    }
                    else
                    {
                        ret = rFail + " " + commandItems[2];
                        WriteLog(ret); return ret;
                    }
                }
                else if (commandItems[0] == cUpdatePassword)
                {
                    WriteLog(command);
                    loc1 = AcceptByID(commandItems[1], commandItems[2]);
                    if (loc1[1] > -1)
                    {
                        bb = bases[loc1[0]].Item(loc1[1]).UpdatePassword(commandItems[2],
                                                                         commandItems[3]);
                        if (bb == true)
                        { /*успех*/
                            ret = rSuccess + " " + commandItems[3];
                            WriteLog(ret); return ret;
                        }
                        else
                        { /*некорректные данные*/
                            ret = rFail + " " + commandItems[3];
                            WriteLog(ret); return ret;
                        }
                    }
                    else
                    {
                        ret = rFail + " " + commandItems[2];
                        WriteLog(ret); return ret;
                    }
                }
                else if (commandItems[0] == cAddMeToActiveInterlocutors)
                {
                    WriteLog(command);
                    loc1 = AcceptByID(commandItems[1], commandItems[2]);
                    if (loc1[1] > -1)
                    {
                        loc2 = FindByID(commandItems[3]);
                        if (loc2[1] > -1)
                        {
                            bases[loc2[0]].Item(loc2[1]).AddActiveInterlocutor(
                                commandItems[1]); /*успех*/
                            ret = rSuccess + " " + commandItems[3];
                            WriteLog(ret); return ret;
                        }
                        else
                        { /*провал - пользователя не существует*/
                            ret = rFail + " " + commandItems[3];
                            WriteLog(ret);
                            return ret;
                        }
                    }
                    else
                    { /*провал - неверный пароль*/
                        ret = rFail + " " + commandItems[2];
                        WriteLog(ret);
                        return ret;
                    }
                }
                else if (commandItems[0] == cCheckMyActiveInterlocutors)
                {
                    WriteLog(command);
                    loc1 = AcceptByID(commandItems[1], commandItems[2]);
                    if (loc1[1] > -1)
                    {
                        List<string> ai = new List<string>();
                        ai = bases[loc1[0]].Item(loc1[1]).FindActiveInterlocutors();
                        /*успех*/
                        ret = rSuccess;
                        for (int i = 0; i < ai.Count; i++)
                        {
                            ret += " " + ai[i];
                        }
                        WriteLog(ret);
                        return ret;
                    }
                    else
                    { /*провал - неверный пароль*/
                        ret = rFail + " " + commandItems[2];
                        WriteLog(ret);
                        return ret;
                    }
                }
            }
            return rFail;
        }
        private void WriteLog(string log) { }
    }
}  // namespace DMM_Server