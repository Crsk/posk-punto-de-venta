using posk.Models;

namespace posk.BLL
{
    class LogItem : registro
    {
        public string UserName
        {
            get { return UsuarioBLL.GetUserName(usuario_id); }
        }
    }
}
