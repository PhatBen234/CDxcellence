using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Services.Service
{
    public class VisitPlanService : IVisitPlanService
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;

        public VisitPlanService(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<VisitPlanDto> CreateVisitPlanAsync(VisitPlanDto dto)
        {
            var visitPlan = new VisitPlan
            {
                ActorId = dto.ActorId,
                VisitDate = dto.VisitDate,
                VisitTime = dto.VisitTime,
                DistributorId = dto.DistributorId,
                Purpose = dto.Purpose,
                IsConfirmed = dto.IsConfirmed,
                Guests = dto.GuestIds.Select(gId => new VisitPlanGuest { GuestId = gId }).ToList()
            };

            _context.VisitPlans.Add(visitPlan);
            await _context.SaveChangesAsync();
            foreach (var guestId in dto.GuestIds)
            {
                var message = $"You have been invited to a visit plan on {dto.VisitDate:yyyy-MM-dd} at {dto.VisitTime}. Purpose: {dto.Purpose}";
                await _notificationService.CreateNotificationAsync(guestId, message);
            }

            dto.Id = visitPlan.Id;
            return dto;
        }

        public async Task<IEnumerable<VisitPlanDto>> GetAllVisitPlansAsync()
        {
            var visitPlans = await _context.VisitPlans
                .Include(vp => vp.Guests)
                .ToListAsync();

            return visitPlans.Select(vp => new VisitPlanDto
            {
                Id = vp.Id,
                ActorId = vp.ActorId,
                VisitDate = vp.VisitDate,
                VisitTime = vp.VisitTime,
                DistributorId = vp.DistributorId,
                Purpose = vp.Purpose,
                IsConfirmed = vp.IsConfirmed,
                GuestIds = vp.Guests.Select(g => g.GuestId).ToList()
            });
        }

        public async Task<VisitPlanDto?> GetVisitPlanByIdAsync(int id)
        {
            var visitPlan = await _context.VisitPlans
                .Include(vp => vp.Guests)
                .FirstOrDefaultAsync(vp => vp.Id == id);

            if (visitPlan == null) return null;

            return new VisitPlanDto
            {
                Id = visitPlan.Id,
                ActorId = visitPlan.ActorId,
                VisitDate = visitPlan.VisitDate,
                VisitTime = visitPlan.VisitTime,
                DistributorId = visitPlan.DistributorId,
                Purpose = visitPlan.Purpose,
                IsConfirmed = visitPlan.IsConfirmed,
                GuestIds = visitPlan.Guests.Select(g => g.GuestId).ToList()
            };
        }

        public async Task<VisitPlanDto?> UpdateVisitPlanAsync(int id, VisitPlanDto dto)
        {
            var visitPlan = await _context.VisitPlans
                .Include(vp => vp.Guests)
                .FirstOrDefaultAsync(vp => vp.Id == id);

            if (visitPlan == null) return null;

            visitPlan.VisitDate = dto.VisitDate;
            visitPlan.VisitTime = dto.VisitTime;
            visitPlan.DistributorId = dto.DistributorId;
            visitPlan.Purpose = dto.Purpose;
            visitPlan.IsConfirmed = dto.IsConfirmed;

            var previousGuestIds = visitPlan.Guests.Select(g => g.GuestId).ToList();
            visitPlan.Guests.Clear();
            visitPlan.Guests.AddRange(dto.GuestIds.Select(gId => new VisitPlanGuest { GuestId = gId }));

            await _context.SaveChangesAsync();


            var newGuestIds = dto.GuestIds.Except(previousGuestIds);
            foreach (var newGuestId in newGuestIds)
            {
                var message = $"You have been invited to a visit plan on {dto.VisitDate:yyyy-MM-dd} at {dto.VisitTime}. Purpose: {dto.Purpose}";
                await _notificationService.CreateNotificationAsync(newGuestId, message);
            }

            dto.Id = visitPlan.Id;
            return dto;
        }

        public async Task<bool> DeleteVisitPlanAsync(int id)
        {
            var visitPlan = await _context.VisitPlans.FindAsync(id);
            if (visitPlan == null) return false;

            _context.VisitPlans.Remove(visitPlan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
