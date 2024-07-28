using Microsoft.EntityFrameworkCore;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.Extensions;
using UsaWeb.Service.Features.Requests;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.ViewModels;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Implementations
{
    public class SurgicalSiteInfectionRepository : ISurgicalSiteInfectionRepository
    {
        private readonly Usaweb_DevContext _context;

        public SurgicalSiteInfectionRepository(Usaweb_DevContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.SurgicalSiteInfection>> GetSurgicalSiteInfectionsAsync(SurgicalSiteInfectionRequest request)
        {
            var query = _context.SurgicalSiteInfections.AsQueryable();

            if (request.SurgicalSiteInfectionId.HasValue)
                query = query.Where(x => x.SurgicalSiteInfectionId == request.SurgicalSiteInfectionId.Value);

            if (!string.IsNullOrEmpty(request.PatientFirstName))
                query = query.Where(x => x.PatientFirstName.Contains(request.PatientFirstName));

            if (!string.IsNullOrEmpty(request.PatientLastName))
                query = query.Where(x => x.PatientLastName.Contains(request.PatientLastName));

            if (!string.IsNullOrEmpty(request.Sex))
                query = query.Where(x => x.Sex == request.Sex);

            if (request.CreateTs.HasValue)
                query = query.Where(x => x.CreateTs == request.CreateTs.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

            return await query.ToListAsync();
        }

        public async Task<Models.SurgicalSiteInfection> CreateSurgicalSiteInfectionAsync(SurgicalSiteInfectionViewModel model)
        {
            var entity = model.ToEntity();
            await _context.SurgicalSiteInfections.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteSurgicalSiteInfectionAsync(int id)
        {
            var entity = await _context.SurgicalSiteInfections.FindAsync(id);
            if (entity == null) return false;

            _context.SurgicalSiteInfections.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Models.SurgicalSiteInfection>> GetAllSurgicalSiteInfectionsAsync()
        {
            return await _context.SurgicalSiteInfections.ToListAsync();
        }

        public async Task<Models.SurgicalSiteInfection> GetSurgicalSiteInfectionByIdAsync(int id)
        {
            return await _context.SurgicalSiteInfections.FindAsync(id);
        }

        public async Task<Models.SurgicalSiteInfection> UpdateSurgicalSiteInfectionAsync(Models.SurgicalSiteInfection entity)
        {
            _context.SurgicalSiteInfections.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
