using SmartTicket.Business.Result;
using SmartTicket.Entities;
using SmartTicket.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTicket.Business
{
    public class UserManager:ManagerBase<User>
    {
        //MailHelper
        
        public BusinessLayerResult<User> Login(User model)
        {
            BusinessLayerResult<User> res = new BusinessLayerResult<User>();
            res.Result = Find(a => a.Username == model.Username && a.Password == model.Password);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            else if (res.Result.Password!=model.Password)
            {
                res.AddError(ErrorMessageCode.BadPassword, "Hatalı şifre");
            }
            return res;
        }

        public BusinessLayerResult<User> Register(User model)
        {
            BusinessLayerResult<User> res = new BusinessLayerResult<User>();

            res.Result = model;
          

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.RegisterNull, "Kayıt yapılamadı.");
            }
            else if (res.Result==Find(x=>x.Username==model.Username))
            {
                res.AddError(ErrorMessageCode.UsernameIsUsed, "Kullanıcı adı mevcut."); 
            }
            else if (res.Result == Find(x => x.Mail == model.Mail))
            {
                res.AddError(ErrorMessageCode.MailIsUsed, "E-Posta mevcut.");
            }
            else if (res.Result == Find(x => x.Phone == model.Phone))
            {
                res.AddError(ErrorMessageCode.PhoneIsUsed, "Telefon numarası mevcut.");
            }
            res.Result.Role = Role.User;
            res.Result.State = State.PendingApproval;
            res.Result.ActivedGuid = Guid.NewGuid();

            return res;

        }





    }
}
