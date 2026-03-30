using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await userManager.Users.OrderBy(u => u.Email).ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.Email,
                    Roles = roles.ToList()
                });
            }            

            return Ok(userList);
        }

        [Authorize(Policy =  "RequireAdminRole")]
        [HttpPost("edit-roles/{userId}")]
        public async Task<ActionResult> EditRoles(string userId, [FromQuery]string roles)
        {
            if (string.IsNullOrEmpty(roles))
                return BadRequest("You must enter at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest("Could not retrieve user");

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            return Ok(await unitOfWork.PhotoRepository.GetUnapprovedPhotos());
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo == null )
                return BadRequest("Could not get photo");

            var member = await unitOfWork.MemberRepository.GetMemberByIdAsync(photo.MemberId);

            if (member == null)
                return BadRequest("Could not get member from photo");

            photo.Approved = true;

            if (member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }

            await unitOfWork.Complete();

            return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo == null )
                return BadRequest("Could not get photo");

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Result.Equals("ok"))
                    unitOfWork.PhotoRepository.RemovePhoto(photo);
            }
            else
            {
                unitOfWork.PhotoRepository.RemovePhoto(photo);
            }

            await unitOfWork.Complete();

            return Ok();
        }
    }
}