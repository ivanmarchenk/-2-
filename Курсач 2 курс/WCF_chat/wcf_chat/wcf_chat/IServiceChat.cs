using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    // Интерфейс нашей службы (фактически описание того, что может делать наш сервис)
    [ServiceContract(CallbackContract =typeof(IServerChatCallback))]
    //данный атрибут представляет собой договоренность о том, что может делать наш сервис
    //сюда передается название интерфейса обратного взаимодействия сервис-клиент
    public interface IServiceChat
    {
        [OperationContract]
        //всякий метод с таким аттрибутом будет виден со стороны клиента

        int Connect(string name);
        //с помощью этого метода мы подключаемся к нашему сервису

        [OperationContract]
        void Disconnect(int id);
        //с помощью этого метода мы отключаемся от нашего сервиса (id клиента, который отключился)

        [OperationContract(IsOneWay = true)]
        //этот параметр показывает что нам не нужно дожидаться ответа от сервера
        void SendMessage(string msg, int id);
        //этот метод принимает сообщения, которые посылает клиент, а также id юзера, чтобы мы могли написать остальным юзерам, кто автор сообщения
        
    }

    //функция обратного взаимодействия (чтобы сервер со своей стороны мог реализовать действия на стороне клиента)
    //иными словами то, как пользователь получит сообщения от сервера
    public interface IServerChatCallback 
    {
        [OperationContract(IsOneWay = true)]
        //если без IsOneWay, то сервер будет ждать подтверждения от клиента
        void MessageCallback(string msg);
    }
}
