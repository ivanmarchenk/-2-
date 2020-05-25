using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
//Класс, реализующий интерфейс нашей службы (как он это будет делать)
namespace wcf_chat 
{
    //здесь описаны все методы нашего интерфейса

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    //данный атрибут указывает, что сервис будет взаимодействовать со всеми пользователями одновременно
    
    public class ServiceChat : IServiceChat
    {
        
        List<ServerUser> users = new List<ServerUser>();
        //список объектов ServerUser
        int nextID = 1;
        //генерация примитивного ID

        public int Connect(string name)
        {
            ServerUser user = new ServerUser() { 
                ID = nextID,
                Name = name,
                OperationContext = OperationContext.Current
                //значения для этого поля будем брать из соответствующего класса методом Current
            };
            //метод создания нового юзера

            nextID++;
            //ID следующего юзера
            SendMessage(": " + user.Name + " подключился к чату!", 0);
            //отправляем другим юзерам сообщение о том, что новый юзер подключился к чату
            users.Add(user);
            //добавляем нового юзера к списку существующих
            return user.ID;

        }

        public void Disconnect(int id)
            //сообщаем сервису, чтобы тот отключил юзера
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            //ищем юзера, которого будем убирать, сравнивая id всех юзеров с id юзера которого хотим удалить
            if (user!=null) //если такого юзера нет, то юзер=налл
            {
                users.Remove(user);//удаляем найденного юзера
                SendMessage(": " + user.Name + " покинул чат!", 0);
                //отправляем другим юзерам сообщение о том, что юзер покинул чат
                //значение 0 показывает, что мы будем рассылать сообщение о случившемся всем юзерам, кроме отключившегося
            }
        }

        public void SendMessage(string msg, int id)
        {
            foreach (var item in users)
                //перебираем список юзеров и формируем сообщение ответа сервера нашим юзерам
            {
                string answer = DateTime.Now.ToShortTimeString();
                //результат - выводим время текущего сообщения
                var user = users.FirstOrDefault(i => i.ID == id);
                //ищем имя юзера, отправившего сообщение
                if (user != null)
                {
                    answer += ":" + user.Name + "";
                }

                answer += msg;
                //вставляем имя юзера и дату в пересылаемое сообщение

                item.OperationContext.GetCallbackChannel<IServerChatCallback>().MessageCallback(answer);
                //отправляем сформированное сообщение юзеру, с которым мы работаем в цикле foreach 
            }
        }
    }
}
