﻿using Institute_of_fine_arts.Dto;
using Institute_of_fine_arts.Entities;
using Institute_of_fine_arts.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IO;
using System;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Institute_of_fine_arts.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Net;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Institute_of_fine_arts.Controllers
{
    [Route("api/art")]
    [ApiController]
    [Authorize(Policy = "Auth")]
    public class ArtsController : ControllerBase
    {
        private readonly InstituteOfFineArtsContext _context;
        private readonly IEmailService _emailService;
        public ArtsController(InstituteOfFineArtsContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("upload")]
        [Authorize(Policy = "Student")]
        public IActionResult uploadImage(IFormFile? image)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest("No image uploaded");
                }
                if (image.ContentType != "image/jpeg" && image.ContentType != "image/jpg" && image.ContentType != "image/png")
                {
                    return BadRequest("Invalid image type");
                }
                var path = "wwwroot/uploads/arts";
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                var upload = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
                image.CopyTo(new FileStream(upload, FileMode.Create));
                var rs = $"{Request.Scheme}://{Request.Host}/uploads/arts/{fileName}";
                var response = new uploadDto
                {
                    Rs = rs,
                    Path = path + "/" + fileName,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "Student")]
        public IActionResult createArt([FromBody] createArtDto? createArt)
        {
            try
            {
                Guid slug = Guid.NewGuid();
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return Unauthorized();
                var user = UserHelper.GetUserDataDto(identity);
                if (user == null) return Unauthorized();
                var a = _context.Arts.FirstOrDefault(a => a.CompetitionId == createArt.CompetitionId && a.OwnerId == user.Id);
                var competition = _context.Competitions.FirstOrDefault(x => x.Id == createArt.CompetitionId);
                if (a == null)
                {
                    if (createArt.Name != "" && createArt.Description != "" && createArt.Url != "" && createArt.Path != "")
                    {
                        var art = new Entities.Art
                        {
                            Name = createArt.Name,
                            IsSell = createArt.IsSell,
                            Price = createArt.Price,
                            Description = createArt.Description,
                            Url = createArt.Url,
                            Path = createArt.Path,
                            Status = "Submitedd",
                            CompetitionId = createArt.CompetitionId,
                            OwnerId = user.Id.Value,
                            CreatedAt = DateTime.Now,
                        };
                        var emailDto = new EmailDto
                        {
                            To = user.Email,
                            Subject = $"Submit the test successfully {competition.Name}",
                            Body = $"you submitted your entry in the painting contest {competition.Name}with the information:" +
                           $"<br/> Name:{createArt.Name}" +
                           $"<br/>Description:{createArt.Description}" +
                           $"<br/>IsSell:{createArt.IsSell}" +
                           $"<br/>Price:{createArt.Price}",
                            Name = user.Name,
                            Url = createArt.Path,
                        };


                        _emailService.SendEmail(emailDto);
                        _context.Arts.Add(art);
                        _context.SaveChanges();
                        var s = _context.Arts.FirstOrDefault(a => a.CompetitionId == createArt.CompetitionId && a.OwnerId == user.Id);
                        s.Slug = $"{competition.Name} - E{s.Id}";
                        _context.SaveChanges();


                        return Ok();
                    }
                    else
                    {
                        return BadRequest("The field is requied");
                    }

                }
                else
                {
                    a.Name = createArt?.Name != "" ? createArt?.Name : a.Name;
                    a.Description = createArt.Description != "" ? createArt.Description : a.Description;
                    a.IsSell = createArt.IsSell;
                    a.Price = createArt.Price != default(decimal) ? createArt.Price : a.Price;
                    a.Url = createArt.Url != "" ? createArt.Url : a.Url;
                    a.Path = createArt.Path != "" ? createArt?.Path : a?.Path;
                    a.UpdatedAt = DateTime.Now;
                    var emailDto = new EmailDto
                    {
                        To = user.Email,
                        Subject = $"Update the test successfully {competition.Name}",
                        Body = $"you updated your entry in the painting contest {competition.Name} with the information:" +
                     $"<br/> Name:{createArt.Name}" +
                     $"<br/>Description:{createArt.Description}" +
                     $"<br/>IsSell:{createArt.IsSell}" +
                     $"<br/>Price:{createArt.Price}" +
                     $"<br/>Updated at:{a.UpdatedAt}",
                        Name = user.Name,
                        Url = createArt?.Path,
                    };
                    _emailService.SendEmail(emailDto);
                    _context.SaveChanges();

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Route("update")]
        [Authorize(Policy = "Student")]
        public IActionResult updateArt(updateArtDto updateArt)
        {
            using (var transaction = _context.Database.BeginTransaction())
                try
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity == null) { return BadRequest(); }
                    var user = UserHelper.GetUserDataDto(identity);
                    if (user == null) { return BadRequest(); }
                    var a = _context.Arts
                        .FirstOrDefault(a =>
                        a.OwnerId == user.Id &&
                        a.CompetitionId == updateArt.CompetitionId);
                    if (a != null)
                    {
                        if (updateArt.Url == null && updateArt.Path == null)
                        {
                            a.Name = updateArt.Name ?? a.Name;
                            a.Slug = updateArt.Slug ?? a.Slug;
                            a.Description = updateArt.Description ?? a.Description;
                            a.IsSell = updateArt.IsSell != null ? updateArt.IsSell : a.IsSell;
                            a.Price = updateArt.Price != null ? updateArt.Price : a.Price;
                            a.UpdatedAt = DateTime.Now;
                        }
                        else
                        {

                            a.Name = updateArt.Name ?? a.Name;
                            a.Slug = updateArt.Slug ?? a.Slug;
                            a.Description = updateArt.Description ?? a.Description;
                            a.IsSell = updateArt.IsSell != null ? updateArt.IsSell : a.IsSell;
                            a.Price = updateArt.Price != null ? updateArt.Price : a.Price;
                            a.Url = updateArt.Url;
                            a.Path = updateArt.Path;
                            a.UpdatedAt = DateTime.Now;
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return NoContent();
                    }
                    else
                    {
                        return NotFound("Art not found.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, ex.Message);
                }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetArts([FromQuery] getArtsDto dto)
        {
            if (dto.Page == null) dto.Page = 1;
            if (dto.Limit == null) dto.Limit = 30;
            int startIndex = (dto.Page.Value - 1) * dto.Limit.Value;
            int endIndex = dto.Page.Value * dto.Limit.Value;

            List<Art> query = _context.Arts.Include(x => x.Evaluates).ToList();

            if (!string.IsNullOrEmpty(dto.Search))
            {
                string[] parseSearchParams = dto.Search.Split(';');
                List<Dictionary<string, string>> searchText = new List<Dictionary<string, string>>();

                foreach (string searchParam in parseSearchParams)
                {
                    string[] keyValue = searchParam.Split(':');
                    string key = keyValue[0];
                    string value = keyValue[1];
                    if (key == "PrizeId" && value == "!=null")
                    {
                        query = query.Where(art => art.PrizeId != null).ToList();
                    }
                    else
                    {
                        searchText.Add(new Dictionary<string, string>
        {
            { key, value }
        });
                    }
                }

                query = query.Where(product =>
                    searchText.All(search =>
                         product.GetType().GetProperty(search.Keys.First())?.GetValue(product)?.ToString() == search.Values.First()
                    )
                ).ToList();
            }
            List<Art> results = query.Skip(startIndex).Take(dto.Limit.Value).ToList();
            string url = $"/art?search={dto.Search}&limit={dto.Limit}";

            var result = new Dictionary<string, object>
{
    { "data", results }
};

            var pagination = PaginationHelper.paginate(query.Count(), dto.Page.Value, dto.Limit.Value, results.Count, url);
            foreach (var property in pagination.GetType().GetProperties())
            {
                result.Add(property.Name, property.GetValue(pagination));
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("get-one")]
        [AllowAnonymous]
        public IActionResult getOne(int competitionId)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null) { return BadRequest(); }
                var user = UserHelper.GetUserDataDto(identity);
                if (user == null) { return BadRequest(); }
                var data = _context.Arts.FirstOrDefault(a => a.OwnerId == user.Id && a.CompetitionId == competitionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("get-by-slug")]
        [AllowAnonymous]
        public IActionResult getBySlug(string slug)
        {
            try
            {
                var data = _context.Arts.Include(x=>x.Competition).Include(x => x.Prize).FirstOrDefault(a => a.Slug == slug);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("send-email")]
        [AllowAnonymous]
        public IActionResult sendEmail(EmailDto email)
        {
            try
            {
                _emailService.SendEmail(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("download")]
        [AllowAnonymous]
        public IActionResult DownloadImage([FromQuery] string imageUrl, int width, int height)
        {
            using (var webClient = new WebClient())
            {
                byte[] imageBytes = webClient.DownloadData(imageUrl);

                using (var image = Image.Load(imageBytes))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max // Chỉnh sửa chế độ thay đổi kích thước tùy ý
                    }));

                    using (var outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, new JpegEncoder()); // Chỉnh sửa định dạng hình ảnh tùy ý

                        outputStream.Position = 0;

                        return File(outputStream.ToArray(), "image/jpeg", "image.png");
                    }
                }
            }
        }
    }
}

