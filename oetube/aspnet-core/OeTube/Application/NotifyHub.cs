using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.AspNetCore.SignalR;

namespace OeTube.Infrastructure.SignalR
{
    [Authorize]
    public class NotifyHub : AbpHub
    {
        public const string UploadCompleted = "UploadCompleted";
    }
}