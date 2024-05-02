using CoreBus.DTOs;
using CoreBus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CoreBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassangerController : ControllerBase
    {

        private readonly CoreBusDbContext _db;
        private IWebHostEnvironment _e;

        public PassangerController(CoreBusDbContext db, IWebHostEnvironment e)
        {
            _db = db;
            _e = e;
        }

        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            List<Passanger> employees = _db.Passangers.Include(x => x.BusTypes).ToList();  //EgerLoading
            string JsonString = JsonConvert.SerializeObject(employees, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            });
            return Content(JsonString, "application/Json");
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {

            Passanger passanger = _db.Passangers.Include(x => x.BusTypes).FirstOrDefault(x => x.PassangerId == id);

            if (passanger == null)
            {
                return NotFound("Empty data");
            }

            string jsonstring = JsonConvert.SerializeObject(passanger, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            return Content(jsonstring, "application/Json");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassanger(int id)
        {
            var st = await _db.Passangers.FindAsync(id);
            if (st == null)
            {
                return BadRequest("passanger not found.");
            }
            _db.Passangers.Remove(st);
            _db.SaveChanges();
            return Ok("deleted");
        }

        [HttpPost]
        public async Task<IActionResult> PostPassangers([FromForm] Common common)
        {
            string FN = common.ImageName + ".jpg";
            string Url = "\\Upload\\" + FN;
            if (common.ImageFile?.Length > 0)
            {
                if (!Directory.Exists(_e.WebRootPath + "\\Upload\\"))
                {
                    Directory.CreateDirectory(_e.WebRootPath + "\\Upload\\");
                }
                using (FileStream fileStream = System.IO.File.Create(_e.WebRootPath + "\\Upload\\" + common.ImageFile.FileName))
                {
                    common.ImageFile.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
            Passanger passanger = new Passanger();
            passanger.Name = common.Name;
            passanger.IsPaid = common.IsPaid;
            passanger.JournyDate = common.JournyDate;
            passanger.ImageName = FN;
            passanger.ImageUrl = Url;
            _db.Passangers.Add(passanger);
            await _db.SaveChangesAsync();
            var emp = _db.Passangers.FirstOrDefault(x => x.Name == common.Name);
            int stid = emp.PassangerId;
            List<BusType> list = JsonConvert.DeserializeObject<List<BusType>>(common.BusTypes);
            AddExperiences(stid, list);
            await _db.SaveChangesAsync();
            return Ok("Saved.");
        }

        private void AddExperiences(int stid, List<BusType>? list)
        {
            {
                foreach (var item in list)
                {
                    BusType experience = new BusType()
                    {
                        PassangerId = stid,
                        SeatFear = item.SeatFear,
                        TypeName = item.TypeName,
                    };
                    _db.BusTypes.Add(experience);
                    _db.SaveChanges();
                }
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpassanger(int id, [FromForm] Common common)
        {
            var passanger = await _db.Passangers.FindAsync(id);
            if (id != common.PassangerId)
            {
                return BadRequest();
            }
            if (passanger == null)
            {
                return NotFound("passanger not found.");
            }
            string fileName = common.ImageName + ".jpg";
            string imageUrl = "\\Upload\\" + fileName;
            if (common.ImageFile?.Length > 0)
            {
                if (!Directory.Exists(_e.WebRootPath + "\\Upload\\"))
                {
                    Directory.CreateDirectory(_e.WebRootPath + "\\Upload\\");
                }
                using (FileStream fileStream = System.IO.File.Create(_e.WebRootPath + "\\Upload\\" + common.ImageFile.FileName))
                {
                    common.ImageFile.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }

            passanger.Name = common.Name;
            passanger.IsPaid = common.IsPaid;
            passanger.JournyDate = common.JournyDate;
            passanger.ImageName = fileName;
            passanger.ImageUrl = imageUrl;

            var exis = _db.BusTypes.Where(x => x.PassangerId == id);
            _db.BusTypes.RemoveRange(exis);

            List<BusType> list = JsonConvert.DeserializeObject<List<BusType>>(common.BusTypes);
            AddExperiences(passanger.PassangerId, list);
            await _db.SaveChangesAsync();
            return Ok("updated");
        }
    }
}
