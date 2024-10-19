using Microsoft.EntityFrameworkCore;
using UsaWeb.Service.Data;
using UsaWeb.Service.Features.SurgicalSiteInfection.Abstractions;
using UsaWeb.Service.Models;

namespace UsaWeb.Service.Features.SurgicalSiteInfection.Implementations
{
    public class SurgicalSiteInfectionSkinPrepRepository(Usaweb_DevContext context) : ISurgicalSiteInfectionSkinPrepRepository
    {
        private readonly Usaweb_DevContext _context = context;

        public async Task<IEnumerable<SurgicalSiteInfectionSkinPrep>> GetAllAsync()
        {
            return await _context.SurgicalSiteInfectionSkinPreps.ToListAsync();
        }

        public async Task<SurgicalSiteInfectionSkinPrep> GetByIdAsync(int id)
        {
            return await _context.SurgicalSiteInfectionSkinPreps.FindAsync(id);
        }

        public async Task<SurgicalSiteInfectionSkinPrep> GetBySurgicalSiteInfectionIdAsync(int id)
        {
            return await _context.SurgicalSiteInfectionSkinPreps.FirstOrDefaultAsync(_ => _.SurgicalSiteInfectionId == id);
        }

        public async Task AddAsync(SurgicalSiteInfectionSkinPrep entity)
        {
            await _context.SurgicalSiteInfectionSkinPreps.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SurgicalSiteInfectionSkinPrep entity)
        {
            _context.SurgicalSiteInfectionSkinPreps.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.SurgicalSiteInfectionSkinPreps.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
