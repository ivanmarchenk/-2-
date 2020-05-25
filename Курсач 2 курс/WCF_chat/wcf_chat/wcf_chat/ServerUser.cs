using System.ServiceModel;

namespace wcf_chat
{
    class ServerUser
    {
        public int ID { get; set; }
        //ID пользователя

        public string Name { get; set; }
        //Имя пользователя

        public OperationContext OperationContext { get; set; }
    }
}
