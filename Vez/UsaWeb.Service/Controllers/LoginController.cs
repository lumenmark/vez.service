using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Xml.Linq;
using UsaWeb.Service.Data;
using UsaWeb.Service.Helper;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace UsaWeb.Service.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    { 
        [HttpPost]
        public Array Get(Login login)       
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var q = db.Member.FirstOrDefault(x => (x.JNumber == login.userName || x.Email == login.userName) && x.Password == login.password);
                if (q != null) {
                    string appList = "";
                    var memList = db.ApplicationMember.Where(x => x.MemberId == q.MemberId);
                    if (memList.Count() > 0) {
                        var obj = memList.Select(x => x.ApplicationShortName).ToArray();
                        appList = String.Join(":", obj);
                    }                   
                    string[] value = new string[] { q.MemberId.ToString(), DateTime.Now.ToString(), q.FirstName, appList };
                    return value;
                }
                string[] zero = new string[] {"0", DateTime.Now.ToString() };
                return zero;
            }
        }


        [HttpPost("/auth/passwordless")]
        public async Task<IActionResult> passwordless(string email)
        {
            DBHelper.LogError("passwordless");
            ResultMessage result = new ResultMessage();

            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == email).FirstOrDefault();
                if (obj != null)
                {
                    DBHelper.LogError("passwordless - found user");
                    string code = ShortGuid.NewGuid();
                    var emlResult = await SendEmail(obj.Email, obj.LastName + ", " + obj.FirstName, code);
                    obj.AccessCode = code;
                    db.SaveChanges();

                    result.Status = "";
                    result.StatusCode = 0;
                }
                else
                {
                    result.Status = "Email address does not exist.";
                    result.StatusCode = 1;
                }
            }
            return Ok(result);
        }

        [HttpPost("/auth/passwordless-register")]
        public async Task<IActionResult> passwordlessRegister(RegisterPwdLess param)
        {
            ResultMessage result = new ResultMessage();
    
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == param.email).FirstOrDefault();
                if (obj == null)
                {
                    string code = ShortGuid.NewGuid();
                    db.Member.Add(new Member
                    {
                        FirstName = param.firstName,
                        LastName = param.lastName,
                        FullName = param.lastName + ", " + param.firstName,
                        Password = code,
                        Email = param.email,
                        Status = "UNVERIFIED",
                        TagColor = getDefaultColor(),
                        CreateTs = DateTime.Now
                    });
                    db.SaveChanges();

                    var emlResult = await SendEmail(param.email, param.lastName + ", " + param.firstName, code);
                    result.Status = "added";
                    result.StatusCode = 0;
                }
                else
                {
                    result.Status = "Email address already exists.";
                    result.StatusCode = 1;
                }
            }

            return Ok(result);
        }

        [HttpPost("/auth/passwordless-verify")]
        public IActionResult passwordlessVerify(string code)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.AccessCode == code).FirstOrDefault();
                if (obj != null)
                {
                    var allmemApps = db.Application.Where(x => x.Status == "ACTIVE" && x.ApplicationMember.Any(z => z.MemberId == obj.MemberId && z.Status == "ACTIVE")).ToList();

                    obj.AccessCode = null;
                    obj.Status = "VERIFIED";
                    db.SaveChanges();
                    var objReturn = SetMemberCookieFields(obj, allmemApps.ToArray());
                    return Ok(objReturn);
                }
            }
            return NotFound();
        }

        [HttpPost("/auth/active-directory-register")]
        public IActionResult activeDirectoryRegister(RegisterAD param)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == param.email).FirstOrDefault();
                if (obj == null)
                {
                    //var allmemApps = db.Application.Where(x => x.Status == "ACTIVE" && x.ApplicationMember.Any(z => z.MemberId == obj.MemberId && z.Status == "ACTIVE")).ToList();

                    string code = ShortGuid.NewGuid();
                    db.Member.Add(new Member
                    {
                        FullName = param.name,
                        Password = code,
                        Email = param.email,
                        Status = "ACTIVE",
                        TagColor = getDefaultColor(),
                        CreateTs = DateTime.Now
                    });
                    db.SaveChanges();

                    var saveObj = db.Member.FirstOrDefault(x => x.Email == param.email);
                    var objReturn = SetMemberCookieFields(saveObj, null);
                    return Ok(objReturn);
                }
                else
                {
                    var allmemApps = db.Application.Where(x => x.Status == "ACTIVE" && x.ApplicationMember.Any(z => z.MemberId == obj.MemberId && z.Status == "ACTIVE")).ToList();

                    var objReturn = SetMemberCookieFields(obj, allmemApps.ToArray());
                    return Ok(objReturn);
                }
            }
        }


        [HttpPost("/auth/login")]
        public IActionResult login(LoginForm param)
        {
            DBHelper.LogError("login-form");
            ViewMemberWithToken objReturn;

            //var pass = HelperService.GetEncryptedPassword(param.password).Result;


            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == param.email && x.Password == param.password).FirstOrDefault();
                if (obj != null)
                {
                    var allmemApps = db.Application.Where(x => x.Status == "ACTIVE" && x.ApplicationMember.Any(z => z.MemberId == obj.MemberId && z.Status == "ACTIVE")).ToList();

                    DBHelper.LogError("login-form - found user");
                    objReturn = SetMemberCookieFields(obj, allmemApps.ToArray());
                    return Ok(objReturn);
                }
            }
            DBHelper.LogError("login-form - not found user");
            objReturn = new ViewMemberWithToken { memberId = -1};
            return Ok(objReturn);
        }

        [HttpPost("/auth/register")]
        public IActionResult register(RegisterForm model)
        {
            DBHelper.LogError("register-form");
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == model.email).FirstOrDefault();
                if (obj == null)
                {
                    //var pass = HelperService.GetEncryptedPassword(model.password).Result;

                    var objNew =  db.Member.Add(new Member
                    {
                        FirstName = model.firstName,
                        LastName = model.lastName,
                        FullName = model.lastName + " " + model.firstName,
                        Password = model.password,
                        Email = model.email,
                        Status = "ACTIVE",
                        TagColor = getDefaultColor(),
                        CreateTs = DateTime.Now
                    });
                    db.SaveChanges();

                    //var emlResult = await SendEmail(model.email, model.lastName + ", " + model.firstName, model.password);
                    var allmemApps = db.Application.Where(x => x.Status == "ACTIVE" && x.ApplicationMember.Any(z => z.MemberId == objNew.Entity.MemberId && z.Status == "ACTIVE")).ToList();

                    var objReturn = SetMemberCookieFields(objNew.Entity, allmemApps.ToArray());
                    return Ok(objReturn);
                }
            }
            return NotFound();
        }

        private string getDefaultColor()
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
            return color;
        }

        private async Task<bool> SendEmail(string email, string toName, string code)
        {
            var emlResult = await HelperService.SendVerifyEmail(email, toName, code);
            return emlResult;
        }

        private ViewMemberWithToken SetMemberCookieFields(Member obj, Array mApps)
        {
            //create jwt token
            string token = HelperService.GetToken(obj.Email);
          
            var objReturn = new ViewMemberWithToken
            {
                memberId = obj.MemberId,
                jNumber = obj.JNumber,
                firstName = obj.FirstName,
                lastName = obj.LastName,
                fullName = obj.FullName,
                email = obj.Email,
                dept = obj.Dept,
                status = obj.Status,
                tagColor = obj.TagColor,
                jwtToken = token,
                memberApps = mApps
            };
            return objReturn;
        }


        //folowing are old api functions

        [Route("/Member/List")]
        [HttpPost]
        public Array Get(MemberRequest request)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                if (request.dept == "null" || string.IsNullOrEmpty(request.dept))
                    request.dept = null;
                if (string.IsNullOrEmpty(request.status))
                    request.status = null;
                if (request.nameOrJ == "null" || string.IsNullOrEmpty(request.nameOrJ))
                    request.nameOrJ = null;
                else
                    request.nameOrJ = request.nameOrJ.ToUpper();

                var result = db.Member.ToList();
                if (request.status != null)
                    result = result.Where(x => x.Status == request.status).ToList();
                if (request.dept != null)
                    result = result.Where(x => x.Dept == request.dept).ToList();
                if (request.nameOrJ != null)
                    result = result.Where(x => x.FirstName.ToUpper().Contains(request.nameOrJ) || 
                                               x.LastName.ToUpper().Contains(request.nameOrJ) || 
                                               x.JNumber ==request.nameOrJ)
                                    .ToList();

                return result.ToArray();
            }
        }

        [Route("/Member/Dept")]
        [HttpGet]
        public Array GetDept()
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var q = db.Member.Select(x => x.Dept).Distinct().ToArray();
                return q;
            }
        }

        [Route("/Member/{memberid}")]
        [HttpGet]
        public Array Get(int memberid)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var q = db.Member.FirstOrDefault(x => x.MemberId == memberid);
                var list = new List<ViewMember>();
                list.Add(new ViewMember
                {
                    MemberId = q.MemberId,
                    JNumber = q.JNumber,
                    FirstName = q.FirstName,
                    LastName = q.LastName,
                    FullName = q.FullName,
                    Password = q.Password,
                    Email = q.Email,
                    Dept = q.Dept,
                    Status = q.Status,
                    SelectedApplication = db.ApplicationMember.Where(x => x.MemberId == memberid)
                                                .Select(z=>z.ApplicationShortName).ToArray()
                });

                return list.ToArray();
            }
        }

        [HttpPost]
        [Route("/Member/Save")]
        public ResultMessage Post(MemberSave member)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                if (member.editOption == "new")
                {
                    var objIs = db.Member.Where(x => x.JNumber == member.jnumber || x.Email == member.email)
                        .FirstOrDefault();
                    if (objIs != null)
                    {
                        return new ResultMessage { Status = "exist" };
                    }
                }
                var obj = db.Member.Where(x => x.JNumber == member.jnumber).FirstOrDefault();
                if (obj != null)
                {
                    obj.FirstName = member.firstName;
                    obj.LastName = member.lastName;
                    obj.FullName = member.lastName + ", " + member.firstName;
                    obj.Password = member.password;
                    obj.Email = member.email;
                    obj.Dept = member.dept;
                    obj.Status = member.status;
                    db.SaveChanges();

                    foreach (var item in db.ApplicationMember.Where(x=>x.MemberId == member.memberid.Value).ToList()) 
                    {
                        var del = db.ApplicationMember
                                        .FirstOrDefault(x => x.ApplicationMemberId == item.ApplicationMemberId);
                        db.ApplicationMember.Remove(del);
                        db.SaveChanges();
                    }

                    foreach (var item in member.selectedApplication)
                    {
                        db.ApplicationMember.Add(new ApplicationMember { 
                            ApplicationShortName = item,
                            MemberId = member.memberid.Value,
                            Status = "ACTIVE"
                        });
                        db.SaveChanges();
                    }
                }
                else
                {  
                    db.Member.Add(new Member
                    {
                        JNumber = member.jnumber,
                        FirstName = member.firstName,
                        LastName = member.lastName,
                        FullName = member.lastName + ", " + member.firstName,
                        Password = member.password,//HelperService.GetEncryptedPassword(member.password).Result,
                        Email = member.email,
                        Dept = member.dept,
                        Status = member.status
                    });
                    db.SaveChanges();

                    var memb = db.Member.FirstOrDefault(x => x.Email == member.email);

                    foreach (var item in member.selectedApplication)
                    {
                        db.ApplicationMember.Add(new ApplicationMember
                        {
                            ApplicationShortName = item,
                            MemberId = memb.MemberId,
                            Status = "ACTIVE"
                        });
                        db.SaveChanges();
                    }
                }
                return new ResultMessage { Status = "ok" };
            }
        }

        [HttpPost]
        [Route("/Member/Delete")]
        public ResultMessage Delete(MemberSave member)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.MemberId == member.memberid).FirstOrDefault();
                if (obj != null)
                {
                    foreach (var item in db.ApplicationMember.Where(x => x.MemberId == member.memberid.Value).ToList())
                    {
                        var del = db.ApplicationMember
                                        .FirstOrDefault(x => x.ApplicationMemberId == item.ApplicationMemberId);
                        db.ApplicationMember.Remove(del);
                        db.SaveChanges();
                    }

                    db.Member.Remove(obj);
                    db.SaveChanges();
                    return new ResultMessage { Status = "ok" };
                }
                return new ResultMessage { Status = "error" };
            }
        }

        [HttpPost("/auth/forgottenpassword")]
        public IActionResult ForgottenPassword(string email)
        {
            //DBHelper.LogError("forgottenpassword");
            ResultMessage objReturn;
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == email).FirstOrDefault();
                if (obj == null)
                {
                    objReturn = new ResultMessage
                    {
                        StatusCode = 404,
                        Status = "That account does not exist"
                    };
                    return Ok(objReturn);
                }
                else
                {
                    var guid = Guid.NewGuid();
                    obj.ResetGuid = guid;
                    db.SaveChanges();

                    var emlResult = HelperService.SendForgottenEmail(obj.Email,
                                        obj.LastName + ", " + obj.FirstName, guid.ToString());
                    //return emlResult.Result;
                    objReturn = new ResultMessage
                    {
                        StatusCode = 200,
                        Status = "Success"
                    };
                    return Ok(objReturn);

                }
            }
        }

        [Route("/auth/verifyCode")]
        [HttpPost]
        public IActionResult VerifyCode(string code)
        {
            ResultMessage objReturn;
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var q = db.Member.FirstOrDefault(x => x.ResetGuid == Guid.Parse(code));
                if (q != null)
                {
                    objReturn = new ResultMessage
                    {
                        StatusCode = 200,
                        Status = "Success"
                    };
                }
                else
                {
                    objReturn = new ResultMessage
                    {
                        StatusCode = 404,
                        Status = "Invalid"
                    };
                }
                return Ok(objReturn);
            }
        }

        [Route("/auth/changePassword")]
        [HttpPost]
        public IActionResult ChangePassword(string code, string password)
        {
            ResultMessage objReturn;
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var q = db.Member.FirstOrDefault(x => x.ResetGuid == Guid.Parse(code));
                if (q != null)
                {
                    //var pass = HelperService.GetEncryptedPassword(password).Result;
                    q.Password = password;
                    q.ResetGuid = null;
                    db.SaveChanges();

                    objReturn = new ResultMessage
                    {
                        StatusCode = 200,
                        Status = "Success"
                    };
                }
                else
                {
                    objReturn = new ResultMessage
                    {
                        StatusCode = 404,
                        Status = "Invalid"
                    };
                }
                return Ok(objReturn);
            }
        }

    }
}
