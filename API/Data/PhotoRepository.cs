using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PhotoRepository(AppDBContext context) : IPhotoRepository
{
    public async Task<Photo?> GetPhotoById(int id)
    {
        return await context.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<PhotoForApprovalDto>> GetUnapprovedPhotos()
    {
        return await context.Photos.IgnoreQueryFilters()
            .Where(p => p.Approved == false)
            .Select(u => new PhotoForApprovalDto
            {
                Id = u.Id,
                UserId = u.MemberId,
                Url = u.Url,
                IsApproved = u.Approved
            })
            .ToListAsync();
    }

    public void RemovePhoto(Photo photo)
    {
        context.Photos.Remove(photo);        
    }
}