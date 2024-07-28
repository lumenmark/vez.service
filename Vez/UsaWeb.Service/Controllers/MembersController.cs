//using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Linq;
using UsaWeb.Service.Data;
using UsaWeb.Service.Helper;
using UsaWeb.Service.Models;
using UsaWeb.Service.ViewModels;
using static Azure.Core.HttpHeader;

namespace UsaWeb.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private const string memberListCacheKey = "memberList";


        public MembersController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpGet("/refresh")]
        public IActionResult Refresh()
        {   
            _cache.Remove(memberListCacheKey);
            return Ok();
        }

        [HttpGet("/members")]
        public string Get(string firstName, string lastName, 
            string status, string fullName, string email, 
            string excludeIds, int maxRecs = 200,
            string orderBy = " firstName, lastName asc")
        {
            List<Member> _memberList = GetCacheMembers();

            if (!string.IsNullOrEmpty(firstName))
                _memberList = _memberList.Where(x => (x.FirstName != null && x.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase))  ).ToList();
            if (!string.IsNullOrEmpty(lastName))
                _memberList = _memberList.Where(x => (x.LastName != null && x.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)) ).ToList();
            if (!string.IsNullOrEmpty(fullName))
                _memberList = _memberList.Where(x => (x.FullName != null && x.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase)) ).ToList();
            if (!string.IsNullOrEmpty(email))
                _memberList = _memberList.Where(x => (x.Email != null && x.Email.Contains(email, StringComparison.OrdinalIgnoreCase)) ).ToList();

            if (!string.IsNullOrEmpty(status))
            {
                string[] statusArr = status.Split(',');
                _memberList = _memberList.Where(c => statusArr.Contains(c.Status)).ToList();
                //_memberList = _memberList.Where(x => (x.Status != null && x.Status.ToLower() == status.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(excludeIds))
                _memberList = _memberList.Where(x => !excludeIds.Contains(x.MemberId.ToString())).ToList();


            _memberList = _memberList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            _memberList = _memberList.Take(maxRecs).ToList();

            return JsonConvert.SerializeObject(_memberList);
        }

        //[Authorize]
        [HttpGet("/members/list")]
        public string Get(string dept, string status, string name)
        {
            List<Member> _memberList = GetCacheMembers();
            if (!string.IsNullOrEmpty(dept))
                _memberList = _memberList.Where(x => (x.Dept != null && x.Dept.ToLower() == dept.ToLower())  ).ToList();
            if (!string.IsNullOrEmpty(status))
                _memberList = _memberList.Where(x => (x.Status != null && x.Status.ToLower() == status.ToLower()) ).ToList();
            if (!string.IsNullOrEmpty(name))
                _memberList = _memberList.Where(x => (x.FullName != null && x.FullName.ToLower().Contains(name.ToLower()) )).ToList();

            return JsonConvert.SerializeObject(_memberList);

            //string query = string.Empty;
            //if (string.IsNullOrEmpty(dept))
            //    dept = "(NULL is NULL OR dept=NULL) ";
            //else
            //    dept = "('" + dept + "' is NULL OR dept='" + dept + "') ";

            //if (string.IsNullOrEmpty(status))
            //    status = "And (NULL is NULL OR status=NULL) ";
            //else
            //    status = "And ('" + status + "' is NULL OR status='" + status + "') ";

            //if (string.IsNullOrEmpty(name))
            //    name = "And (NULL is NULL OR fullname=NULL) ";
            //else
            //    name = "And ('" + name + "' is NULL OR fullname like '%" + name + "%') ";

            //query = "select * from member where " + dept + status + name +
            //"for json path;";

            //var result = DBHelper.RawSqlQuery(query, null);

            //return result;
        }

        private List<Member> GetCacheMembers()
        {
            List<Member> _memberList = null;
            if (_cache.TryGetValue(memberListCacheKey, out string memberList))
            {
                _memberList = JsonConvert.DeserializeObject<List<Member>>(memberList);
            }
            if (_memberList == null)
            {
                using (Usaweb_DevContext db = new Usaweb_DevContext())
                {
                    _memberList = db.Member.ToList();
                    var list = JsonConvert.SerializeObject(_memberList);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                           .SetSlidingExpiration(TimeSpan.FromDays(1))
                           .SetAbsoluteExpiration(TimeSpan.FromDays(1))
                           .SetPriority(CacheItemPriority.Normal)
                           .SetSize(1024);
                    _cache.Set(memberListCacheKey, list, cacheEntryOptions);
                }
            }
            return _memberList;
        }

        [Route("/members/{memberid}")]
        [HttpGet]
        public Array Get(int memberid)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var q = db.Member.FirstOrDefault(x => x.MemberId == memberid);
                var list = new List<ViewMembers>();

                
                list.Add(new ViewMembers
                {
                    memberId = q.MemberId,
                    jNumber = q.JNumber,
                    firstName = q.FirstName,
                    lastName = q.LastName,
                    fullName = q.FullName,
                    password = q.Password, //HelperService.GetDecryptPassword(q.Password).Result,
                    email = q.Email,
                    dept = q.Dept,
                    status = q.Status,
                    applicationShortNames = db.ApplicationMember.Where(x => x.MemberId == memberid)
                                                .Select(z => z.ApplicationShortName).ToArray()
                });

                return list.ToArray();
            }
        }


        [HttpPost("/members")]
        public IActionResult Post(MemberAddEdit param)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.Email == param.email).FirstOrDefault();
                if (obj == null)
                {
                    var random = new Random();
                    var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
                    
                    if (!string.IsNullOrEmpty(param.tagColor))
                        color = param.tagColor;

                    var newObj = db.Member.Add(new Member
                    {
                        JNumber = param.jNumber,
                        FirstName = param.firstName,
                        LastName = param.lastName,
                        FullName = param.fullName,
                        Password = param.password, //HelperService.GetEncryptedPassword(param.password).Result,
                        Email = param.email,
                        Dept = param.dept,
                        Status = param.status,
                        TagColor = color,
                        CreateTs = DateTime.Now
                    });
                    db.SaveChanges();

                    addShortList(db, param.applicationShort, newObj.Entity.MemberId);
                    return Ok();
                }
                return BadRequest();
            }
        }

        [HttpPut("/members")]
        public IActionResult Put(long memberId, MemberAddEdit param)
        {
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                var obj = db.Member.Where(x => x.MemberId == memberId && x.MemberId != 0).FirstOrDefault();
                if (obj != null)
                {
                    var random = new Random();
                    var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"

                    if (!string.IsNullOrEmpty(param.tagColor))
                        color = param.tagColor;

                    obj.JNumber = param.jNumber;
                    obj.FirstName = param.firstName;
                    obj.LastName = param.lastName;
                    obj.FullName = param.fullName;
                    obj.Password = param.password; //HelperService.GetEncryptedPassword(param.password).Result;
                    obj.Email = param.email;
                    obj.Dept = param.dept;
                    obj.Status = param.status;
                    obj.TagColor = color;
                    db.SaveChanges();

                    var list = db.ApplicationMember.Where(x => x.MemberId == memberId).ToList();
                    foreach (var item in list)
                    {
                        db.ApplicationMember.Remove(item);
                        db.SaveChanges();
                    }
                    addShortList(db, param.applicationShort, memberId);

                    return Ok();
                }
                return BadRequest();
            }
        }

        private void addShortList(Usaweb_DevContext db, string applicationShort, long memberId)
        {
            var arr = applicationShort.Split(',');
            foreach (var item in arr)
            {
                db.ApplicationMember.Add(new ApplicationMember
                {
                    MemberId = Convert.ToInt32(memberId),
                    ApplicationShortName = item.Trim(),
                    Status = "ACTIVE"
                });
                db.SaveChanges();
            }
        }

    }
}
